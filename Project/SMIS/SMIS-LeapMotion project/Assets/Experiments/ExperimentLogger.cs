using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;

[XmlRoot("PositionLogCollection")]
public class ExperimentLogger{
    [System.NonSerialized]
    public int counter = 0;

    public float interval = 0.1f;

    public float meanPenetrationDepth = 0;

    [XmlArray("ShapeLogs")]
    [XmlArrayItem("shapeLog")]
    public List<ShapeLog> shapes = new List<ShapeLog>();

    [XmlArray("TrackedPositionLogs")]
    [XmlArrayItem("TrackedPositionLog")]
    public List<TrackedPositionLog> values = new List<TrackedPositionLog>();
    
}


 
 public class TrackedPositionLog{

    [XmlAttribute("name")]
    public string Name;

    public List<Vector3> positions;
}

public class ShapeLog { 

    [XmlAttribute("type")]
    public string Type;

    public Vector3 shapePosition;
    public Vector3 shapeRotation;
    public string shapeMaterial;
}