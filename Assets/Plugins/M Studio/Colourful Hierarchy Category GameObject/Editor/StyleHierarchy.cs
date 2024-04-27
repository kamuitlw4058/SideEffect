using System;
using UnityEditor;
using UnityEngine;

namespace MStudio
{
    [InitializeOnLoad]
    public class StyleHierarchy
    {
        static string[] dataArray;//Find ColorPalette GUID
        static string path;//Get ColorPalette(ScriptableObject) path
        static ColorPalette colorPalette;

        static StyleHierarchy()
        {
            dataArray = AssetDatabase.FindAssets("t:ColorPalette");

            if (dataArray.Length >= 1)
            {    //We have only one color palette, so we use dataArray[0] to get the path of the file
                path = AssetDatabase.GUIDToAssetPath(dataArray[0]);

                colorPalette = AssetDatabase.LoadAssetAtPath<ColorPalette>(path);

                EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindow;
            }
        }

        static float IconWidth = 16;
        static float IconHeight = 16;

        static int ColorGradientLength = 16;

        private static void OnHierarchyWindow(int instanceID, Rect selectionRect)
        {
            //To make sure there is no error on the first time the tool imported in project
            if (dataArray.Length == 0) return;

            UnityEngine.Object instance = EditorUtility.InstanceIDToObject(instanceID);

            var backRect = new Rect(new Vector2(selectionRect.position.x + IconWidth, selectionRect.y), new Vector2(selectionRect.size.x - IconWidth, selectionRect.size.y));
            var iconRect = new Rect(selectionRect.position, new Vector2(IconWidth, IconHeight));

            Texture2D BackTexture = new Texture2D(256, 1);

            if (instance != null)
            {
                for (int i = 0; i < colorPalette.colorDesigns.Count; i++)
                {
                    var design = colorPalette.colorDesigns[i];

                    //Check if the name of each gameObject is begin with keyChar in colorDesigns list.
                    if (instance.name.StartsWith(design.keyChar))
                    {
                        //Remove the symbol(keyChar) from the name.
                        string newName = instance.name.Substring(design.keyChar.Length);

                        bool FoundTexture = false;

                        foreach (var colorTexture in colorPalette.ColorGradientTexture)
                        {
                            if (colorTexture.Equals(design.backgroundColor))
                            {
                                FoundTexture = true;
                                BackTexture = colorTexture.texture;
                            }
                        }

                        if (!FoundTexture)
                        {
                            EditorGUI.DrawRect(selectionRect, design.backgroundColor);
                            // BackTexture = new Texture2D(ColorGradientLength, 1);
                            // Color[] colors = new Color[ColorGradientLength];
                            // for (int x = 0; x < ColorGradientLength; i++)
                            // {
                            //     colors[x] = new Color(design.backgroundColor.r, design.backgroundColor.g, design.backgroundColor.b, (ColorGradientLength - x) / ColorGradientLength);
                            // }
                            // BackTexture.SetPixels(colors);
                            // BackTexture.Apply();
                            // // GUI.DrawTexture(iconRect, BackTexture);
                            // colorPalette.ColorGradientTexture.Add(new ColorTexture { color = design.backgroundColor, texture = BackTexture });
                            // EditorUtility.SetDirty(colorPalette);
                            // AssetDatabase.SaveAssets();
                        }
                        else
                        {
                            GUI.DrawTexture(iconRect, BackTexture);
                        }

                        // if (!colorPalette.ColorGradientTexture.TryGetValue(design.backgroundColor, out BackTexture))
                        // {
                        //     BackTexture = new Texture2D(256, 1);
                        //     Color[] colors = new Color[256];
                        //     for (int x = 0; x < 256; i++)
                        //     {
                        //         colors[x] = new Color(design.backgroundColor.r, design.backgroundColor.g, design.backgroundColor.b, (256 - x) / 256);
                        //     }
                        //     BackTexture.SetPixels(colors);
                        //     BackTexture.Apply();
                        //     // GUI.DrawTexture(iconRect, BackTexture);
                        //     colorPalette.ColorGradientTexture.Add(design.backgroundColor, BackTexture);
                        //     EditorUtility.SetDirty(colorPalette);
                        //     AssetDatabase.SaveAssets();
                        // }
                        // GUI.DrawTexture(iconRect, BackTexture);
                        // else
                        // {
                        //     BackTexture = new Texture2D(256, 1);
                        //     Color[] colors = new Color[256];
                        //     for (int x = 0; x < 256; i++)
                        //     {
                        //         colors[x] = new Color(design.backgroundColor.r, design.backgroundColor.g, design.backgroundColor.b, (256 - x) / 256);
                        //     }
                        //     BackTexture.SetPixels(colors);
                        //     BackTexture.Apply();
                        //     GUI.DrawTexture(iconRect, BackTexture);
                        //     colorPalette.ColorGradientTexture.Add(design.backgroundColor, BackTexture);
                        //     EditorUtility.SetDirty(colorPalette);
                        //     AssetDatabase.SaveAssets();
                        // }

                        //Draw a rectangle as a background, and set the color.
                        // EditorGUI.DrawRect(selectionRect, design.backgroundColor);
                        // EditorGUI.DrawRect(selectionRect, design.backgroundColor);


                        //Create a new GUIStyle to match the desing in colorDesigns list.
                        GUIStyle newStyle = new GUIStyle
                        {
                            alignment = design.textAlignment,
                            fontStyle = design.fontStyle,
                            normal = new GUIStyleState()
                            {
                                textColor = design.textColor,
                            }
                        };

                        if (design.Upper)
                        {
                            newName = newName.ToUpper();
                        }

                        //Draw a label to show the name in upper letters and newStyle.
                        //If you don't like all capital latter, you can remove ".ToUpper()".
                        EditorGUI.LabelField(selectionRect, newName, newStyle);

                        if (design.Icon != null)
                        {
                            GUI.DrawTexture(iconRect, design.Icon);
                        }
                    }
                }
            }
        }
    }
}