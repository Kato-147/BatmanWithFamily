using UnityEngine;

internal class Instantiate
{
    private GameObject gameObject;
    private Vector3 vector3;
    private Quaternion identity;

    public Instantiate(GameObject gameObject, Vector3 vector3, Quaternion identity)
    {
        this.gameObject = gameObject;
        this.vector3 = vector3;
        this.identity = identity;
    }
}