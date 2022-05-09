using GameBrains.Extensions.Transforms;
using GameBrains.Extensions.Vectors;
using GameBrains.Visualization;
using UnityEngine;

namespace GameBrains.Entities.EntityData
{
    [System.Serializable]
    public class StaticData : TransformWrapper
    {
	    #region Constructor

	    public StaticData(Transform t) : base(t)
	    {
		    var meshFilter = t.GetComponentInChildren<MeshFilter>();
		    var localScale = t.localScale;
		    Size = meshFilter != null ? Vector3.Scale(localScale, meshFilter.mesh.bounds.size) : localScale;
		    
		    Radius = 0.5f;
		    Height = 2;
		    CenterOffset = VectorXYZ.up * (Height / 2);

		    SetupObstacleLayerMask();
	    }

        #endregion

        #region Copy constructor

        public StaticData(StaticData staticDataSource) : base(staticDataSource)
        {
	        Size = staticDataSource.Size;
	        Radius = staticDataSource.Radius;
	        Height = staticDataSource.Height;
	        CenterOffset = staticDataSource.CenterOffset;

	        ObstacleLayerMask = staticDataSource.ObstacleLayerMask;
        }

        #endregion

        #region Obstacles

        [SerializeField]
        LayerMask obstacleLayerMask;

        public LayerMask ObstacleLayerMask
        {
	        get => obstacleLayerMask;
	        set => obstacleLayerMask = value;
        }
        public int ObstacleLayer { get; protected set; } = 0;

        public bool IsObstacle
        {
	        get => WrappedTransform.gameObject.layer == ObstacleLayer;
	        set => WrappedTransform.gameObject.layer = value ? ObstacleLayer : 0;
        }


        void SetupObstacleLayerMask()
        {
	        ObstacleLayer = LayerMask.NameToLayer("Obstacle");
	        if (ObstacleLayer == -1)
	        {
		        Debug.LogError("Obstacle layer not defined. Using Default layer instead.");
		        ObstacleLayer = 0;
	        }

	        ObstacleLayerMask = 1 << ObstacleLayer;
        }

        #endregion Obstacles

        public static implicit operator StaticData(Transform t) { return new StaticData(t); }

        #region Dimensions
        
        public Vector3 Size { get; set; } // based on mesh or local scale

        // Default dimensions based on capsule with height 2 and radius 0.5

        public float Radius { get; set; }
        public float Height { get; set; }
        public VectorXYZ CenterOffset { get; set; }

        public VectorXYZ Top => Center + VectorXYZ.up * Height * Radius;

        public VectorXYZ Bottom => Center - VectorXYZ.up * Height * Radius;

        public VectorXYZ Center => CenterOffset + Position;

        #endregion

        #region Utility methods

	    #region Close enough and far enough distances and angles

	    public float CloseEnoughDistance { get; set; } = 1;

	    public float FarEnoughDistance { get; set; } = 10;

	    public float CloseEnoughAngle { get; set; } = 5; // degrees

	    public float FarEnoughAngle { get; set; } = 22.5f; // degrees

	    #endregion Close enough and far enough distances and angles

	    #region Ray and Capsule cast colors

	    public Color ClearColor { get; set; } = Color.green;

	    public Color BlockedColor { get; set; } = Color.red;

	    #endregion Ray and Capsule cast colors

		#region IsAtPosition

		public bool IsAtPosition(VectorXYZ position)
		{
			return IsAtPosition(position, CloseEnoughDistance);
		}

		public bool IsAtPosition(
			VectorXYZ position,
			float closeEnoughDistance)
		{
			return (Position - position).magnitude <= closeEnoughDistance;
		}

		#endregion IsAtPosition

		#region HasLineOfSight

		public bool HasLineOfSight(
			VectorXYZ position,
			RayCastVisualizer rayCastVisualizer = null,
			bool showVisualizer = false,
			bool showOnlyWhenBlocked = false,
			Color? clearColor = null,
			Color? blockedColor = null)
		{
			return HasLineOfSight(
				position,
				out RaycastHit hitInfo,
				rayCastVisualizer,
				showVisualizer,
				showOnlyWhenBlocked,
				clearColor,
				blockedColor);
		}

		public bool HasLineOfSight(
			VectorXYZ position,
			out RaycastHit hitInfo,
			RayCastVisualizer rayCastVisualizer = null,
			bool showVisualizer = false,
			bool showOnlyWhenBlocked = false,
			Color? clearColor = null,
			Color? blockedColor = null)
		{
			if (clearColor == null) clearColor = ClearColor;
			if (blockedColor == null) blockedColor = BlockedColor;

			bool blocked = Physics.Raycast(
				Center,
				(position - Position).normalized,
				out hitInfo,
				(position - Position).magnitude,
				ObstacleLayerMask);

			if (rayCastVisualizer)
			{
				if (showVisualizer && (!showOnlyWhenBlocked || blocked))
				{
					rayCastVisualizer.SetColor(blocked ? blockedColor.Value : clearColor.Value);

					rayCastVisualizer.Draw(
					Center,
					(position - Position).normalized,
					blocked ? hitInfo.distance : (position - Position).magnitude);
				}
				else
				{
					rayCastVisualizer.Hide(true);
				}
			}

			return !blocked;
		}

		#endregion HasLineOfSight

		// #region Can move checks
		//
		// #region CanMoveTo
		//
		// public bool CanMoveTo(
		// 	VectorXYZ endPosition,
		// 	CapsuleCastVisualizer capsuleCastVisualizer = null,
		// 	bool showVisualizer = false,
		// 	bool showOnlyWhenBlocked = false,
		// 	float castRadiusMultiplier = 1.5f,
		// 	Color? clearColor = null,
		// 	Color? blockedColor = null)
		// {
		// 	return CanMoveTo(
		// 		endPosition,
		// 		out RaycastHit hitInfo,
		// 		capsuleCastVisualizer,
		// 		showVisualizer,
		// 		showOnlyWhenBlocked,
		// 		castRadiusMultiplier,
		// 		clearColor,
		// 		blockedColor);
		// }
		//
		// public bool CanMoveTo(
		// 	VectorXYZ endPosition,
		// 	out RaycastHit hitInfo,
		// 	CapsuleCastVisualizer capsuleCastVisualizer = null,
		// 	bool showVisualizer = false,
		// 	bool showOnlyWhenBlocked = false,
		// 	float castRadiusMultiplier = 1.5f,
		// 	Color? clearColor = null,
		// 	Color? blockedColor = null)
		// {
		// 	return CanMoveBetween(
		// 		Position,
		// 		endPosition,
		// 		out hitInfo,
		// 		capsuleCastVisualizer,
		// 		showVisualizer,
		// 		showOnlyWhenBlocked,
		// 		castRadiusMultiplier,
		// 		clearColor,
		// 		blockedColor);
		// }
		//
		// public bool CanMoveTo(
		// 	VectorXYZ endPosition,
		// 	LayerMask obstacleLayers,
		// 	CapsuleCastVisualizer capsuleCastVisualizer = null,
		// 	bool showVisualizer = false,
		// 	bool showOnlyWhenBlocked = false,
		// 	float castRadiusMultiplier = 1.5f,
		// 	Color? clearColor = null,
		// 	Color? blockedColor = null)
		// {
		// 	return CanMoveTo(
		// 		endPosition,
		// 		obstacleLayers,
		// 		out RaycastHit hitInfo,
		// 		capsuleCastVisualizer,
		// 		showVisualizer,
		// 		showOnlyWhenBlocked,
		// 		castRadiusMultiplier,
		// 		clearColor,
		// 		blockedColor);
		// }
		//
		// public bool CanMoveTo(
		// 	VectorXYZ endPosition,
		// 	LayerMask obstacleLayers,
		// 	out RaycastHit hitInfo,
		// 	CapsuleCastVisualizer capsuleCastVisualizer = null,
		// 	bool showVisualizer = false,
		// 	bool showOnlyWhenBlocked = false,
		// 	float castRadiusMultiplier = 1.5f,
		// 	Color? clearColor = null,
		// 	Color? blockedColor = null)
		// {
		// 	return CanMoveBetween(
		// 		Position,
		// 		endPosition,
		// 		obstacleLayers,
		// 		out hitInfo,
		// 		capsuleCastVisualizer,
		// 		showVisualizer,
		// 		showOnlyWhenBlocked,
		// 		castRadiusMultiplier,
		// 		clearColor,
		// 		blockedColor);
		// }
		//
		// #endregion CanMoveTo
		//
		// #region CanMoveBetween
		//
		// public bool CanMoveBetween(
		// 	VectorXYZ startPosition,
		// 	VectorXYZ endPosition,
		// 	CapsuleCastVisualizer capsuleCastVisualizer = null,
		// 	bool showVisualizer = false,
		// 	bool showOnlyWhenBlocked = false,
		// 	float castRadiusMultiplier = 1.5f,
		// 	Color? clearColor = null,
		// 	Color? blockedColor = null)
		// {
		// 	return CanMoveBetween(
		// 		startPosition,
		// 		endPosition,
		// 		out RaycastHit hitInfo,
		// 		capsuleCastVisualizer,
		// 		showVisualizer,
		// 		showOnlyWhenBlocked,
		// 		castRadiusMultiplier,
		// 		clearColor,
		// 		blockedColor);
		// }
		//
		// public bool CanMoveBetween(
		// 	VectorXYZ startPosition,
		// 	VectorXYZ endPosition,
		// 	out RaycastHit hitInfo,
		// 	CapsuleCastVisualizer capsuleCastVisualizer = null,
		// 	bool showVisualizer = false,
		// 	bool showOnlyWhenBlocked = false,
		// 	float castRadiusMultiplier = 1.5f,
		// 	Color? clearColor = null,
		// 	Color? blockedColor = null)
		// {
		// 	return CanMoveBetween(
		// 		startPosition,
		// 		endPosition,
		// 		ObstacleLayerMask,
		// 		out hitInfo,
		// 		capsuleCastVisualizer,
		// 		showVisualizer,
		// 		showOnlyWhenBlocked,
		// 		castRadiusMultiplier,
		// 		clearColor,
		// 		blockedColor);
		// }
		//
		// public bool CanMoveBetween(
		// 	VectorXYZ startPosition,
		// 	VectorXYZ endPosition,
		// 	LayerMask obstacleLayers,
		// 	CapsuleCastVisualizer capsuleCastVisualizer = null,
		// 	bool showVisualizer = false,
		// 	bool showOnlyWhenBlocked = false,
		// 	float castRadiusMultiplier = 1.5f,
		// 	Color? clearColor = null,
		// 	Color? blockedColor = null)
		// {
		// 	return CanMoveBetween(
		// 		startPosition,
		// 		endPosition,
		// 		obstacleLayers,
		// 		out RaycastHit hitInfo,
		// 		capsuleCastVisualizer,
		// 		showVisualizer,
		// 		showOnlyWhenBlocked,
		// 		castRadiusMultiplier,
		// 		clearColor,
		// 		blockedColor);
		// }
		//
		// public bool CanMoveBetween(
		// 	VectorXYZ startPosition,
		// 	VectorXYZ endPosition,
		// 	LayerMask obstacleLayers,
		// 	out RaycastHit hitInfo,
		// 	CapsuleCastVisualizer capsuleCastVisualizer = null,
		// 	bool showVisualizer = false,
		// 	bool showOnlyWhenBlocked = false,
		// 	float castRadiusMultiplier = 1.5f,
		// 	Color? clearColor = null,
		// 	Color? blockedColor = null)
		// {
		// 	if (clearColor == null) clearColor = ClearColor;
		// 	if (blockedColor == null) blockedColor = BlockedColor;
		//
		// 	float distance
		// 		= VectorXYZ.Distance(startPosition, endPosition);
		// 	VectorXYZ direction
		// 		= (endPosition - startPosition) / distance;
		// 	float radius = Radius;
		//
		// 	// VectorXYZ capsuleBottomSphereCenterPosition
		// 	// 	= Bottom + radius * VectorXYZ.up;
		// 	// VectorXYZ capsuleTopSphereCenterPosition
		// 	// 	= Top - radius * VectorXYZ.up;
		// 	
		// 	VectorXYZ capsuleBottomSphereCenterPosition
		// 		= startPosition + Radius * VectorXYZ.up;
		// 	VectorXYZ capsuleTopSphereCenterPosition
		// 		= startPosition + (Height - Radius) * VectorXYZ.up;
		//
		// 	float castRadius = Radius * castRadiusMultiplier;
		//
		// 	bool blocked = Physics.CapsuleCast(
		// 		capsuleBottomSphereCenterPosition,
		// 		capsuleTopSphereCenterPosition,
		// 		castRadius,
		// 		direction,
		// 		out hitInfo,
		// 		distance,
		// 		obstacleLayers);
		//
		// 	if (capsuleCastVisualizer)
		// 	{
		// 		if (showVisualizer && (!showOnlyWhenBlocked || blocked))
		// 		{
		// 			capsuleCastVisualizer.SetColor(blocked ? blockedColor.Value : clearColor.Value);
		// 			capsuleCastVisualizer.castRadiusMultiplier = castRadiusMultiplier;
		//
		// 			capsuleCastVisualizer.Draw(
		// 			startPosition,
		// 			direction,
		// 			blocked ? hitInfo.distance : distance);
		// 		}
		// 		else
		// 		{
		// 			capsuleCastVisualizer.Hide(true);
		// 		}
		// 	}
		//
		// 	return !blocked;
		// }
		//
		// #endregion CanMoveBetween
		//
		// #region CanStepRight
		//
		// public bool CanStepRight(
		// 	CapsuleCastVisualizer capsuleCastVisualizer = null,
		// 	bool showVisualizer = false,
		// 	bool showOnlyWhenBlocked = false,
		// 	float castRadiusMultiplier = 1.5f,
		// 	Color? clearColor = null,
		// 	Color? blockedColor = null)
		// {
		// 	return CanStepRight(
		// 		ObstacleLayerMask,
		// 		capsuleCastVisualizer,
		// 		showVisualizer,
		// 		showOnlyWhenBlocked,
		// 		castRadiusMultiplier,
		// 		clearColor,
		// 		blockedColor);
		// }
		//
		// public bool CanStepRight(
		// 	LayerMask obstacleLayers,
		// 	CapsuleCastVisualizer capsuleCastVisualizer = null,
		// 	bool showVisualizer = false,
		// 	bool showOnlyWhenBlocked = false,
		// 	float castRadiusMultiplier = 1.5f,
		// 	Color? clearColor = null,
		// 	Color? blockedColor = null)
		// {
		// 	return CanStepRight(obstacleLayers,
		// 		out VectorXYZ positionOfStep,
		// 		capsuleCastVisualizer,
		// 		showVisualizer,
		// 		showOnlyWhenBlocked,
		// 		castRadiusMultiplier,
		// 		clearColor,
		// 		blockedColor);
		// }
		//
		// public bool CanStepRight(
		// 	out VectorXYZ positionOfStep,
		// 	CapsuleCastVisualizer capsuleCastVisualizer = null,
		// 	bool showVisualizer = false,
		// 	bool showOnlyWhenBlocked = false,
		// 	float castRadiusMultiplier = 1.5f,
		// 	Color? clearColor = null,
		// 	Color? blockedColor = null)
		// {
		// 	return CanStepRight(
		// 		ObstacleLayerMask,
		// 		out positionOfStep,
		// 		capsuleCastVisualizer,
		// 		showVisualizer,
		// 		showOnlyWhenBlocked,
		// 		castRadiusMultiplier,
		// 		clearColor,
		// 		blockedColor);
		// }
		//
		// public bool CanStepRight(
		// 	LayerMask obstacleLayers,
		// 	out VectorXYZ positionOfStep,
		// 	CapsuleCastVisualizer capsuleCastVisualizer = null,
		// 	bool showVisualizer = false,
		// 	bool showOnlyWhenBlocked = false,
		// 	float castRadiusMultiplier = 1.5f,
		// 	Color? clearColor = null,
		// 	Color? blockedColor = null)
		// {
		// 	float stepDistance = Radius * 2;
		//
		// 	positionOfStep
		// 		= Position + stepDistance * (VectorXYZ)WrappedTransform.right;
		//
		// 	return CanMoveTo(
		// 		positionOfStep,
		// 		obstacleLayers,
		// 		capsuleCastVisualizer,
		// 		showVisualizer,
		// 		showOnlyWhenBlocked,
		// 		castRadiusMultiplier,
		// 		clearColor,
		// 		blockedColor);
		// }
		//
		// #endregion CanStepRight
		//
		// #region CanStepLeft
		//
		// public bool CanStepLeft(
		// 	CapsuleCastVisualizer capsuleCastVisualizer = null,
		// 	bool showVisualizer = false,
		// 	bool showOnlyWhenBlocked = false,
		// 	float castRadiusMultiplier = 1.5f,
		// 	Color? clearColor = null,
		// 	Color? blockedColor = null)
		// {
		// 	return CanStepLeft(
		// 		ObstacleLayerMask,
		// 		capsuleCastVisualizer,
		// 		showVisualizer,
		// 		showOnlyWhenBlocked,
		// 		castRadiusMultiplier,
		// 		clearColor,
		// 		blockedColor);
		// }
		//
		// public bool CanStepLeft(
		// 	LayerMask obstacleLayers,
		// 	CapsuleCastVisualizer capsuleCastVisualizer = null,
		// 	bool showVisualizer = false,
		// 	bool showOnlyWhenBlocked = false,
		// 	float castRadiusMultiplier = 1.5f,
		// 	Color? clearColor = null,
		// 	Color? blockedColor = null)
		// {
		// 	return CanStepLeft(
		// 		obstacleLayers,
		// 		out VectorXYZ positionOfStep,
		// 		capsuleCastVisualizer,
		// 		showVisualizer,
		// 		showOnlyWhenBlocked,
		// 		castRadiusMultiplier,
		// 		clearColor,
		// 		blockedColor);
		// }
		//
		// public bool CanStepLeft(
		// 	out VectorXYZ positionOfStep,
		// 	CapsuleCastVisualizer capsuleCastVisualizer = null,
		// 	bool showVisualizer = false,
		// 	bool showOnlyWhenBlocked = false,
		// 	float castRadiusMultiplier = 1.5f,
		// 	Color? clearColor = null,
		// 	Color? blockedColor = null)
		// {
		// 	return CanStepLeft(
		// 		ObstacleLayerMask,
		// 		out positionOfStep,
		// 		capsuleCastVisualizer,
		// 		showVisualizer,
		// 		showOnlyWhenBlocked,
		// 		castRadiusMultiplier,
		// 		clearColor,
		// 		blockedColor);
		// }
		//
		// public bool CanStepLeft(
		// 	LayerMask obstacleLayers,
		// 	out VectorXYZ positionOfStep,
		// 	CapsuleCastVisualizer capsuleCastVisualizer = null,
		// 	bool showVisualizer = false,
		// 	bool showOnlyWhenBlocked = false,
		// 	float castRadiusMultiplier = 1.5f,
		// 	Color? clearColor = null,
		// 	Color? blockedColor = null)
		// {
		// 	float stepDistance = Radius * 2;
		//
		// 	positionOfStep
		// 		= Position - (stepDistance * (VectorXYZ)WrappedTransform.right);
		//
		// 	return CanMoveTo(
		// 		positionOfStep,
		// 		obstacleLayers,
		// 		capsuleCastVisualizer,
		// 		showVisualizer,
		// 		showOnlyWhenBlocked,
		// 		castRadiusMultiplier,
		// 		clearColor,
		// 		blockedColor);
		// }
		//
		// #endregion CanStepLeft
		//
		// #endregion Can move checks

		#endregion
    }
}