using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ResCostScritableObject))]
public class DirectEditResCostEditor : Editor
{
    private eResCostFunctionType lastFunctionType;
    private ResCostScritableObject resCostObject;

    private void OnEnable()
    {
        resCostObject = target as ResCostScritableObject;
        lastFunctionType = resCostObject.functionType;
    }

    public override void OnInspectorGUI()
    {
        // 绘制基础属性
        resCostObject.resType = (eResType)EditorGUILayout.EnumPopup("资源类型", resCostObject.resType);
        resCostObject.functionType =
            (eResCostFunctionType)EditorGUILayout.EnumPopup("函数类型", resCostObject.functionType);

        // 检测类型变化
        if (resCostObject.functionType != lastFunctionType)
        {
            CreateMethodInstance();
            lastFunctionType = resCostObject.functionType;
        }

        // 绘制方法参数
        DrawMethodParameters();

        // 强制保存修改
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private void CreateMethodInstance()
    {
        switch (resCostObject.functionType)
        {
            case eResCostFunctionType.Exponential:
                resCostObject.method = new ExponentialResCostMethod(eCellType.Black,1,1);
                break;
            case eResCostFunctionType.Linear:
                resCostObject.method = new LinearResCostMethod(eCellType.Black, 1, 1);
                break;
            // 添加其他类型处理...
        }
    }

    private void DrawMethodParameters()
    {
        if (resCostObject.method == null) CreateMethodInstance();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("函数参数", EditorStyles.boldLabel);

        switch (resCostObject.method)
        {
            case ExponentialResCostMethod exponential:
                DrawExponentialParameters(exponential);
                break;
            case LinearResCostMethod linear:
                DrawLinearParameters(linear);
                break;
            // 添加其他参数绘制逻辑...
        }
    }

    private void DrawExponentialParameters(ExponentialResCostMethod method)
    {
        EditorGUILayout.LabelField("F = 系数x底数^（数量）细胞类型");
        // 使用直接字段访问
        method.type = (eCellType)EditorGUILayout.EnumPopup("细胞类型", method.type);
        method.k = EditorGUILayout.IntField("系数", method.k);
        method.baseNum = EditorGUILayout.FloatField("底数", method.baseNum);
    }

    private void DrawLinearParameters(LinearResCostMethod method)
    {
        EditorGUILayout.LabelField("F = 系数x（数量）细胞类型 + 偏移");
        // 使用直接字段访问
        method.type = (eCellType)EditorGUILayout.EnumPopup("细胞类型", method.type);
        method.k = EditorGUILayout.FloatField("系数", method.k);
        method.b = EditorGUILayout.IntField("偏移", method.b);
    }
}