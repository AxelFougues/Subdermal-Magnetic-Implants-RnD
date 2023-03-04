using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Xml.Serialization;

public class ExperimentLoggerManager : MonoBehaviour{

    public string saveName = "shapeExpLog";
    public float interval = 0.1f;
    public List<GameObject> tracked;
    public IEnumerator recording = null;
    

    ExperimentLogger logger = new ExperimentLogger();

    public void start() {
        StartCoroutine("startLogging");
    }

    public void stop() {
        logger.counter++;
        Save(Path.Combine(Application.dataPath, saveName + logger.counter + ".xml"));
        Debug.Log("Log saved as " + Path.Combine(Application.dataPath, saveName + logger.counter + ".xml"));
        logger.values.Clear();
        StopCoroutine(recording);
        recording = null;
    }

    public void displayLast() {
        if (recording != null) return;
        var loadedLogger = Load(Path.Combine(Application.dataPath, saveName + logger.counter + ".xml"));
        foreach (TrackedPositionLog log in loadedLogger.values) {
            GameObject line = new GameObject(log.Name + " trajectory");
            line.transform.parent = transform;
            LineRenderer lineRenderer = line.AddComponent<LineRenderer>() as LineRenderer;
            lineRenderer.positionCount = log.positions.Count;
            lineRenderer.SetPositions(log.positions.ToArray());
            lineRenderer.startWidth = lineRenderer.endWidth = 0.006f;
            lineRenderer.startColor = lineRenderer.endColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f, 1f, 1f);
            lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        }
    }

    protected static ExperimentLogger Load(string path) {
        var serializer = new XmlSerializer(typeof(ExperimentLogger));
        using (var stream = new FileStream(path, FileMode.Open)) {
            return serializer.Deserialize(stream) as ExperimentLogger;
        }
    }

    protected void Save(string path) {
        var serializer = new XmlSerializer(typeof(ExperimentLogger));
        using (var stream = new FileStream(path, FileMode.Create)) {
            serializer.Serialize(stream, logger);
        }
    }

    IEnumerator startLogging() {
        //Check if all tracked are present
        bool valid = true;
        Debug.Log("Waiting for tracked objects to be in the scene");
        do {
            if (Input.GetKeyDown(KeyCode.S)) break;
            valid = true;
            foreach (GameObject o in tracked) if (o == null) valid = false;
            yield return new WaitForSeconds(interval);
        } while (!valid);

        if (valid) {
            Debug.Log("Started tracking");
            //Initialise logs and logger
            logger.interval = interval;
            for(int i = 0; i < transform.childCount; i++) {
                logger.shapes.Add(new ShapeLog());
                logger.shapes[logger.shapes.Count - 1].Type = transform.GetChild(i).name;
                logger.shapes[logger.shapes.Count - 1].shapePosition = transform.GetChild(i).position;
                logger.shapes[logger.shapes.Count - 1].shapeRotation = transform.GetChild(i).rotation.eulerAngles;
                if(transform.GetChild(i).GetComponent<TouchableObject>() != null) logger.shapes[logger.shapes.Count - 1].shapeMaterial = transform.GetChild(i).GetComponent<TouchableObject>().materialProperties.name;
            }
            logger.values.Clear();
            foreach (GameObject o in tracked) {
                logger.values.Add(new TrackedPositionLog());
                logger.values[logger.values.Count - 1].Name = o.name + "/" + o.transform.parent.name + "/" + o.transform.parent.parent.name;
                logger.values[logger.values.Count - 1].positions = new List<Vector3>();
            }
            recording = doLogging();
            StartCoroutine(recording);
        } else Debug.Log("Tracking aborted");
    }

    IEnumerator doLogging() {
        while (true) { 
            for (int i = 0; i < tracked.Count; i++) {
                if (tracked[i] != null) logger.values[i].positions.Add(tracked[i].transform.position);
            }
            yield return new WaitForSeconds(interval);
        }
    }

}
