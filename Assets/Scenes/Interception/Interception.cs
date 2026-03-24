using UnityEngine; // Nécessite Unity pour Vector3 (ou crée ton propre struct si besoin)

//Truec bizarre, si la target se déplace VERS le missile, mais que sa vitesse est + élevée, l'algo va considérer que la collision est impossible

public static class Interception
{
    /// <summary>
    /// Calcule le vecteur de vitesse que doit prendre l'objet A pour intercepter l'objet B.
    /// </summary>
    /// <param name="posA">Position de l'objet A</param>
    /// <param name="posB">Position de l'objet B</param>
    /// <param name="velB">Vitesse de l'objet B</param>
    /// <param name="speedA">Vitesse (scalaire) de l'objet A</param>
    /// <returns>Vecteur vitesse à appliquer à A</returns>
    public static Vector3 CalculateInterceptVelocity(Vector3 posA, Vector3 posB, Vector3 velB, float speedA)
    {
        Vector3 direction = posB - posA;
        float distance = direction.magnitude;

        float speedB = velB.magnitude;
        float angle = Vector3.Angle(direction, velB) * Mathf.Deg2Rad;

        float t = SolveInterceptionTime(distance, speedA, speedB, angle);

        if (float.IsNaN(t) || t <= 0f)
        {
            Debug.Log("Impossible to intercept");
            // Impossible d'intercepter, alors aller vers la position actuelle
            return direction.normalized * speedA;
        }

        Vector3 futurePosB = posB + velB * t;
        Vector3 desiredDirection = (futurePosB - posA).normalized;
        return desiredDirection * speedA;
    }

    /// <summary>
    /// Calcule le temps d'interception basé sur une équation quadratique.
    /// </summary>
    private static float SolveInterceptionTime(float d, float speedA, float speedB, float angle)
    {
        // Résolution basée sur la loi des cosinus :
        // t = d / sqrt(speedA^2 - speedB^2 * sin^2(angle))
        float denom = speedA * speedA - speedB * speedB * Mathf.Sin(angle) * Mathf.Sin(angle);

        if (denom <= 0)
            return float.NaN;

        float t = d / Mathf.Sqrt(denom);
        return t;
    }
}

/*
//Exemple d'utilisation
Vector3 posA = new Vector3(0, 0, 0);
Vector3 posB = new Vector3(10, 0, 0);
Vector3 velB = new Vector3(1, 1, 0);
float speedA = 5f;

Vector3 interceptionVelocity = Interception.CalculateInterceptVelocity(posA, posB, velB, speedA);
*/