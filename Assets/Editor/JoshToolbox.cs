using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//using System;
using Codice.CM.Common.Serialization;

#if UNITY_EDITOR

public class JoshToolbox : EditorWindow
{
    bool randomTranslateX=true, randomTranslateY=true, randomScaleX=true, randomScaleY=true;

    float minTranslate=-5, maxTranslate=5, minScale=.9f, maxScale=1.1f, minRotate=-15, maxRotate=15, minH=0, minS=0, minV=1, maxH=1, maxS=1, maxV=1;

    int layer=0;

    Vector2 scrollPos = Vector2.zero;
    
    [MenuItem("Tools/Josh Toolbox")]

    [CanEditMultipleObjects]

    public static void ShowWindow()
    {
        GetWindow<JoshToolbox>("Josh Toolbox");
    }

    void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        GUILayout.Label("Josh Toolbox", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUILayout.Label("Random Translate:", EditorStyles.boldLabel);

        randomTranslateX = EditorGUILayout.Toggle("X", randomTranslateX);
        randomTranslateY = EditorGUILayout.Toggle("Y", randomTranslateY);

        minTranslate = EditorGUILayout.FloatField("Min", minTranslate);
        maxTranslate = EditorGUILayout.FloatField("Max", maxTranslate);

        if(GUILayout.Button("Randomize Translate"))
        {
            translatebtn();
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUILayout.Label("Random Scale:", EditorStyles.boldLabel);

        randomScaleX = EditorGUILayout.Toggle("X", randomScaleX);
        randomScaleY = EditorGUILayout.Toggle("Y", randomScaleY);

        minScale = EditorGUILayout.FloatField("Min", minScale);
        maxScale = EditorGUILayout.FloatField("Max", maxScale);

        if(GUILayout.Button("Randomize Scale"))
        {
            scalebtn();
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUILayout.Label("Random Rotate:", EditorStyles.boldLabel);

        minRotate = EditorGUILayout.FloatField("Min", minRotate);
        maxRotate = EditorGUILayout.FloatField("Max", maxRotate);

        if(GUILayout.Button("Randomize Rotation"))
        {
            rotatebtn();
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUILayout.Label("Random Flip:", EditorStyles.boldLabel);

        if(GUILayout.Button("Random Flip X"))
        {
            flipbtn("x");
        }
        if(GUILayout.Button("Random Flip Y"))
        {
            flipbtn("y");
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUILayout.Label("Random HSV:", EditorStyles.boldLabel);

        GUILayout.Label("Hue:", EditorStyles.boldLabel);
        minH = EditorGUILayout.Slider("Min", minH,0,maxH);
        maxH = EditorGUILayout.Slider("Max", maxH,minH,1);

        GUILayout.Label("Saturation:", EditorStyles.boldLabel);
        minS = EditorGUILayout.Slider("Min", minS,0,maxS);
        maxS = EditorGUILayout.Slider("Max", maxS,minS,1);

        GUILayout.Label("Brightness:", EditorStyles.boldLabel);
        minV = EditorGUILayout.Slider("Min", minV,0,maxV);
        maxV = EditorGUILayout.Slider("Max", maxV,minV,1);

        if(GUILayout.Button("Randomize HSV"))
        {
            hsvbtn();
        }
    
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUILayout.Label("Object Alignment:", EditorStyles.boldLabel);

        if(GUILayout.Button("Average Position X"))
        {
            avgPosition("x");
        }
        if(GUILayout.Button("Average Position Y"))
        {
            avgPosition("y");
        }

        if(GUILayout.Button("Even X Spacing"))
        {
            evenSpacing("x");
        }

        if(GUILayout.Button("Even Y Spacing"))
        {
            evenSpacing("y");
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        GUILayout.Label("Order:", EditorStyles.boldLabel);

        layer = EditorGUILayout.IntField("Layer", layer);

        if(GUILayout.Button("Set Layer Order"))
        {
            setLayer(layer);
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("");

        EditorGUILayout.EndScrollView();
    }

    void translatebtn()
    {
        foreach(GameObject obj in Selection.gameObjects)
        {   
            if(randomTranslateX)
            {
                obj.transform.position = new Vector2(obj.transform.position.x+Random.Range(minTranslate,maxTranslate), obj.transform.position.y);
            }

            if(randomTranslateY)
            {
                obj.transform.position = new Vector2(obj.transform.position.x, obj.transform.position.y+Random.Range(minTranslate,maxTranslate));
            }            
        }
    }
    
    void scalebtn()
    {
        foreach(GameObject obj in Selection.gameObjects)
        {   
            float uniformScale = Random.Range(minScale,maxScale);

            if(randomScaleX)
            {
                obj.transform.localScale = new Vector2(obj.transform.localScale.x*uniformScale, obj.transform.localScale.y);
            }

            if(randomScaleY)
            {
                obj.transform.localScale = new Vector2(obj.transform.localScale.x, obj.transform.localScale.y*uniformScale);
            }            
        }
    }

    void rotatebtn()
    {
        foreach(GameObject obj in Selection.gameObjects)
        {   
            obj.transform.eulerAngles = new Vector3(obj.transform.eulerAngles.x, obj.transform.eulerAngles.y, obj.transform.eulerAngles.z+Random.Range(minRotate,maxRotate));
        }
    }

    void flipbtn(string xy)
    {
        foreach(GameObject obj in Selection.gameObjects)
        {   
            if(xy=="x")
            {
                int chance = Random.Range(1,3);

                if(chance==1)
                {
                    obj.transform.eulerAngles = new Vector3(obj.transform.eulerAngles.x+180, obj.transform.eulerAngles.y, obj.transform.eulerAngles.z);
                }
            }

            if(xy=="y")
            {
                int chance = Random.Range(1,3);

                if(chance==1)
                {
                    obj.transform.eulerAngles = new Vector3(obj.transform.eulerAngles.x, obj.transform.eulerAngles.y+180, obj.transform.eulerAngles.z);
                }
                
            }
        }
    }

    void hsvbtn()
    {
        foreach(GameObject obj in Selection.gameObjects)
        {
            SpriteRenderer mysprite = obj.GetComponent<SpriteRenderer>();

            mysprite.color = Color.HSVToRGB(Random.Range(minH,maxH), Random.Range(minS,maxS), Random.Range(minV,maxV));
        }
    }

    void avgPosition(string xory="x")
    {
        float total=0, denominator=0;

        if(xory=="x")
        {
            foreach(GameObject obj in Selection.gameObjects)
            {
                total += obj.transform.position.x;
                denominator++;
            }

            foreach(GameObject obj in Selection.gameObjects)
            {
                obj.transform.position = new Vector3(total/denominator, obj.transform.position.y);
            }
        }
        else if(xory=="y")
        {
            foreach(GameObject obj in Selection.gameObjects)
            {
                total += obj.transform.position.y;
                denominator++;
            }

            foreach(GameObject obj in Selection.gameObjects)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, total/denominator);
            }
        }
    }

    void evenSpacing(string xory="x")
    {
        float min=0, max=0, denominator=0, spacing=0, newPos=0;

        if(xory=="x")
        {
            foreach(GameObject obj in Selection.gameObjects)
            {
                if(min>obj.transform.position.x)
                {
                    min = obj.transform.position.x;
                }

                if(max<=obj.transform.position.x)
                {
                    max = obj.transform.position.x;
                }

                denominator++;
            }

            spacing = (max-min)/denominator;

            foreach(GameObject obj in Selection.gameObjects)
            {   
                obj.transform.position = new Vector3(newPos, obj.transform.position.y);
                newPos += spacing;
            }
        }
        else if(xory=="y")
        {
            foreach(GameObject obj in Selection.gameObjects)
            {
                if(min>obj.transform.position.y)
                {
                    min = obj.transform.position.y;
                }

                if(max<=obj.transform.position.y)
                {
                    max = obj.transform.position.y;
                }

                denominator++;
            }

            spacing = (max-min)/denominator;

            foreach(GameObject obj in Selection.gameObjects)
            {   
                obj.transform.position = new Vector3(obj.transform.position.x, newPos);
                newPos += spacing;
            }
        }
    }

    void setLayer(int layer)
    {
        foreach(GameObject obj in Selection.gameObjects)
        {
            obj.GetComponent<Renderer>().sortingOrder = layer;
        }
    }

    //     if(stringVar == string.Empty)
    //     {
    //         Debug.LogError("No Empty Fields!");
    //         return;
    //     }
}

#endif