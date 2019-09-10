using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker
{
    public List<Vector2> path = new List<Vector2>();
    public Vector2 position;
    public float theta;
    public float omega;
    public float maxOmegaMagnitude;
    public float maxAlphaMagnitude;
    public System.Random random;
    public Vector2? goalPosition;
    public float goalWeight;
    public float goalMetDistance;

    public Walker(int seed, float startTheta, float startOmega,
      float maxOmegaMagnitude, float maxAlphaMagnitude, Vector2 startPosition,
      Vector2? goalPosition = null, float goalWeight = 1, float goalMetDistance = 1f)
    {
        path.Add(startPosition);
        position = startPosition;
        theta = startTheta;
        omega = startOmega;
        this.maxOmegaMagnitude = maxOmegaMagnitude;
        this.maxAlphaMagnitude = maxAlphaMagnitude;
        this.random = new System.Random(seed);
        this.goalPosition = goalPosition;
        this.goalWeight = goalWeight;
        this.goalMetDistance = goalMetDistance;
    }
    public void Walk(float totalTime, float deltaTime = 1)
    {
        for (float time = 0; time < totalTime; time += deltaTime)
        {
            if(goalPosition.HasValue && (goalPosition-position).Value.magnitude < goalMetDistance) break;

            float deltaOmega = 0;
            float deltaTheta = 0;

            deltaOmega += maxAlphaMagnitude * Tools.RandomFloat(random, -1, 1) * deltaTime;

            if (goalPosition.HasValue)
            {
                Vector2 goalDirection = (goalPosition.Value-position).normalized;
                float correctionAlpha = Vector2.SignedAngle(Tools.UnitVector(theta), goalDirection);
                deltaTheta += correctionAlpha * goalWeight * deltaTime;
            }
            omega += deltaOmega;
            omega = Tools.BoundedFloat(omega, -maxOmegaMagnitude, maxOmegaMagnitude);

            deltaTheta += omega * deltaTime;
            theta += deltaTheta;
            Vector2 direction = Tools.UnitVector(theta);

            position += direction * deltaTime;
            path.Add(position);
        }
    }
}
