using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Tools {
    //[MenuItem("Tools/TestA")]
    //static void TestA() {
    //    Debug.Log("TestA方法被执行");
    //}
    //[MenuItem("Tools/TestB")]
    //static void TestB() {
    //    Debug.Log("TestB方法被执行");
    //}
    //[MenuItem("Tools/TestC")]
    //static void TestC() {
    //    Debug.Log("TestC方法被执行");
    //}
    //[MenuItem("Tools/TestD")]
    //static void TestD() {
    //    Debug.Log("TestD方法被执行");
    //}

    /// <summary>
    /// 自定义菜单栏分级
    /// </summary>
    //第三个参数为priority优先级,自定义创建的每一个菜单栏默认优先级为1000
    [MenuItem("Tools/TestE",false,3)]
    static void TestE() {
        Debug.Log("TestE方法被执行");
    }
    [MenuItem("Tools/TestF", false, 4)]
    static void TestF() {
        Debug.Log("TestF方法被执行");
    }
    [MenuItem("Tools/TestG", false, 15)]
    static void TestG() {
        Debug.Log("TestG方法被执行");
    }
    /// <summary>
    /// GameObject
    /// </summary>
    [MenuItem("GameObject/TestH", false, 10)]
    static void TestH() {
        Debug.Log("TestH方法被执行");
    }
    /// <summary>
    /// Assets
    /// </summary>
    [MenuItem("Assets/TestI")]
    static void TestI() {
        Debug.Log("TestI方法被执行");
    }
}
