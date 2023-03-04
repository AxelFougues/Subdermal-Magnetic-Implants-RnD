using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Spawner : MonoBehaviour {
    public abstract void spawn();

    void printList(List<GameObject> list) {
        foreach (GameObject go in list) {
            Debug.Log(go.name);
        }
    }
}


public class ShapeSpawner : Spawner {
    public List<GameObject> shapes;
    public List<TouchableMaterialProperties> materials;
    [Range(1, 3)]
    public int shapeCount = 2;
    public bool allowDuplicates = false;
    public bool randomRotation;
    public bool randomMaterials;
    [Range(0.05f, 0.6f)]
    public float scale = 0.5f;

    public override void spawn() {
        //clean up
        if (transform.childCount > 0) foreach(Transform child in transform) if(child.tag == "Spawnable") Destroy(child.gameObject);
        //spawn
        Quaternion rotation = randomRotation ? Random.rotation : transform.rotation;
        switch(shapeCount){
            case 1: {
                    GameObject go = Instantiate(shapes[Random.Range(0, shapes.Count)], transform.position, rotation, transform);
                    go.transform.localScale = go.transform.localScale * 0.5f;
                    if (randomMaterials) go.GetComponent<TouchableObject>().materialProperties = materials[Random.Range(0, materials.Count)];
                    break;
                }
            case 2: {
                    List<TouchableMaterialProperties> usableMats = new List<TouchableMaterialProperties>(materials);
                    List<GameObject> usableShapes = new List<GameObject>(shapes);

                    int index = Random.Range(0, usableShapes.Count);
                    GameObject go = Instantiate(usableShapes[index], transform.position + Vector3.right * 0.2f, rotation, transform);
                    if (!allowDuplicates && usableShapes.Count > 1) usableShapes.RemoveAt(index);
                    go.transform.localScale = go.transform.localScale * 0.5f;

                    rotation = randomRotation ? Random.rotation : transform.rotation;

                    index = Random.Range(0, usableShapes.Count);
                    GameObject go1 = Instantiate(usableShapes[index], transform.position - Vector3.right * 0.2f, rotation, transform);
                    go1.transform.localScale = go1.transform.localScale * 0.5f;

                    if (randomMaterials) {
                        index = Random.Range(0, usableMats.Count);
                        go.GetComponent<TouchableObject>().materialProperties = usableMats[index];
                        if (usableMats.Count > 1) usableMats.RemoveAt(index);
                        index = Random.Range(0, usableMats.Count);
                        go1.GetComponent<TouchableObject>().materialProperties = usableMats[index];
                    }
                    break;
                }
            case 3: {
                    List<TouchableMaterialProperties> usableMats = new List<TouchableMaterialProperties>(materials);
                    List<GameObject> usableShapes = new List<GameObject>(shapes);

                    int index = Random.Range(0, usableShapes.Count);
                    GameObject go = Instantiate(usableShapes[index], transform.position, rotation, transform);
                    if (!allowDuplicates && usableShapes.Count > 1) usableShapes.RemoveAt(index);
                    go.transform.localScale = go.transform.localScale * 0.5f;

                    rotation = randomRotation ? Random.rotation : transform.rotation;

                    index = Random.Range(0, usableShapes.Count);
                    GameObject go1 = Instantiate(usableShapes[index], transform.position + Vector3.right * 0.2f, rotation, transform);
                    if (!allowDuplicates && usableShapes.Count > 1) usableShapes.RemoveAt(index);
                    go1.transform.localScale = go1.transform.localScale * 0.5f;

                    rotation = randomRotation ? Random.rotation : transform.rotation;

                    index = Random.Range(0, usableShapes.Count);
                    GameObject go2 = Instantiate(usableShapes[index], transform.position - Vector3.right * 0.2f, rotation, transform);
                    go2.transform.localScale = go2.transform.localScale * 0.5f;

                    if (randomMaterials) {
                        index = Random.Range(0, usableMats.Count);
                        go.GetComponent<TouchableObject>().materialProperties = usableMats[index];
                        if (usableMats.Count > 1) usableMats.RemoveAt(index);
                        index = Random.Range(0, usableMats.Count);
                        go1.GetComponent<TouchableObject>().materialProperties = usableMats[index];
                        if (usableMats.Count > 1) usableMats.RemoveAt(index);
                        index = Random.Range(0, usableMats.Count);
                        go2.GetComponent<TouchableObject>().materialProperties = usableMats[index];
                    }
                    break;
                }
        }

    }

}
