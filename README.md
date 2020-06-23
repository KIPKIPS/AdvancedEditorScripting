# Advanced_Editor_Scripting
Unity扩展编辑器工具类方法

工具记录:
(1)十六进制颜色转成Color对象:
Color Str2Color(string value) {
        byte r = 0;
        byte g = 0;
        byte b = 0;

        byte.TryParse(value.Substring(0, 2), System.Globalization.NumberStyles.HexNumber, null, out r);
        byte.TryParse(value.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, null, out g);
        byte.TryParse(value.Substring(4, 2), System.Globalization.NumberStyles.HexNumber, null, out b);

        return new Color((float)r / 255, (float)g / 255, (float)b / 255, 1);
    }

(2)
<1>清理预制体上挂载组件的方法,不同于实例化的游戏对象的组件清理方法(直接Destory Component即可)
清理预制体的基本思路,先将预制体实例化一份加载到场景中去,在场景中使用实例化游戏物体的删除组件的方法,
将删除组件的游戏对象应用到预制体上即可
代码:通用组件删除,这种方法会将组件存储下来,保存到预制体下方
//清理组件 思路将预制体实例化一份到场景中,修改,完了之后覆盖本地预制体
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;
using System;
using Game.Logic;
using System.Collections;
using System.IO;

class ChangeBtnTextColor : EditorWindow
{
    /// <summary>
    /// 批量替换预设里面的文本颜色
    /// hosr
    /// </summary>
    //private static Color newColor;
    private static bool isChange = false;
    [MenuItem("Assets/批量替换按钮上颜色(程序员专用=。=)")]
    public static void OpenEditorWin() {
        ChangeBtnTextColor win = EditorWindow.GetWindow<ChangeBtnTextColor>(true, "批量替换按钮上颜色");
        win.maxSize = new Vector2(350, 150);
        win.minSize = new Vector2(350, 150);
    }

    void OnGUI() {
        GUILayout.BeginVertical();
        GUILayout.Space(5);
        GUILayout.Label("1.先点击选中要操作的预设", GUILayout.Width(300f));
        GUILayout.Label("2.运行时间决定于预设复杂程度,请耐心等待", GUILayout.Width(300f));
        GUILayout.Space(10);
        // oldSprite = EditorGUILayout.ObjectField(oldSprite, typeof(Sprite), false, GUILayout.Width(150f)) as Sprite;
        // GUILayout.Space(5);
        // newSprite = EditorGUILayout.ObjectField(newSprite, typeof(Sprite), false, GUILayout.Width(150f)) as Sprite;
        // newColor = EditorGUILayout.ColorField("新颜色值", newColor);
        // newColor = new Color(newColor.r, newColor.g, newColor.b, 1);
        GUILayout.Space(10);

        if (GUILayout.Button("开始修改")) {
            ChangeSprite();
        }
        GUILayout.EndVertical();
    }
    void ChangeSprite() {
        EditorUtility.DisplayProgressBar("Modify Prefab", "Please wait...", 0);
        if (Selection.gameObjects.Length < 1) {
            Debug.LogError("未选中要操作对象");
            return;
        }

        foreach (GameObject item in Selection.gameObjects) {
            string path = AssetDatabase.GetAssetPath(item);
            if (!path.EndsWith(".prefab"))
                return;
            GameObject cloneObj = PrefabUtility.InstantiatePrefab(item) as GameObject;
            if (cloneObj != null) {
                cloneObj.name = item.name;
                IteratorChild(cloneObj.transform);
            }

            if (isChange) {
                PrefabUtility.ReplacePrefab(cloneObj, item, ReplacePrefabOptions.Default);
                DestroyImmediate(cloneObj);
            }
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    void IteratorChild(Transform t) {
        Text txt = t.GetComponent<Text>();

        if (txt != null) {
            Image parentImage = null;
            if (t.parent != null) {
                parentImage = t.parent.GetComponent<Image>();
            }

            bool change = false;
            bool isYellow = true;
            if (parentImage != null && parentImage.sprite != null) {
                //黄色按钮
                if (parentImage.sprite.name == "c_btn_005") {
                    change = true;
                }
                //蓝色
                else if (parentImage.sprite.name == "c_btn_004") {
                    change = true;
                    isYellow = false;
                }
                if (change) {
                    txt.color = Str2Color("FFFFFFFF"); //文本色置为白色
                    Color targetColor = Str2Color(isYellow ? "A45217FF" : "285F9FFF"); //判断改变的颜色值
                    //OutlineEx组件存在
                    if (t.GetComponent<OutlineEx>() != null) {
                        t.GetComponent<OutlineEx>().OutlineWidth = 1;
                    }
                    //OutlineEx组件丢失
                    else {
                        //添加组件
                        t.gameObject.AddComponent<OutlineEx>();
                        t.GetComponent<OutlineEx>().OutlineWidth = 1;
                    }

                    OutlineEx oe = t.GetComponent<OutlineEx>();
                    oe.ChangeOutlineColor(targetColor); //更换颜色
                    oe.OutlineWidth = 1; //设置描边宽度
                    //移除Outline组件
                    if (t.GetComponent<Outline>() != null) {
                        //Debug.Log("销毁Outline组件");
                        DestroyImmediate(t.GetComponent<Outline>());
                    }
                    isChange = true;
                    Debug.Log(t.name + "  ...........  Done!");
                }
            }
        }
        foreach (Transform child in t) {
            IteratorChild(child);
        }
    }

    Color Str2Color(string value) {
        byte r = 0;
        byte g = 0;
        byte b = 0;

        byte.TryParse(value.Substring(0, 2), System.Globalization.NumberStyles.HexNumber, null, out r);
        byte.TryParse(value.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, null, out g);
        byte.TryParse(value.Substring(4, 2), System.Globalization.NumberStyles.HexNumber, null, out b);

        return new Color((float)r / 255, (float)g / 255, (float)b / 255, 1);
    }
}


