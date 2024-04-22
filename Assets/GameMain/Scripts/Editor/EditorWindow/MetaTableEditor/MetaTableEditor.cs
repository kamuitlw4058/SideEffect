using System.Linq;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Pangoo.MetaTable;
using MetaTable;
using Pangoo;


namespace SideEffect.Editor
{
    public class MetaTableEditor : OdinMenuEditorWindow, IMetaTableEditor
    {
        [MenuItem("SideEffect/MetaTable", false, 10)]
        private static void OpenWindow()
        {
            var window = GetWindow<MetaTableEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1600, 700);
            window.titleContent = new GUIContent("MetaTable");
            window.MenuWidth = 250;
        }

        private void OnSelectionChange()
        {
            // Debug.Log($"Selection:{}");
        }

        protected override object GetTarget()
        {
            Debug.Log($"Target:{Selection.activeObject}");
            return base.GetTarget();
        }
        protected override void OnBeginDrawEditors()
        {
            if (MenuTree == null)
                return;

            var toolbarHeight = MenuTree.Config.SearchToolbarHeight;

            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                // GUILayout.Label("提交拉取前务必点击保存全部配置");


                if (SirenixEditorGUI.ToolbarButton(new GUIContent("刷新菜单树")))
                {
                    ForceBuildMenuTree();
                    ForceMenuTreeRebuild();
                }
                if (SirenixEditorGUI.ToolbarButton(SdfIconType.ReplyFill))
                {
                    Debug.Log($"SelectionStack.Count:{SelectionStack.Count}");
                    if (SelectionStack.Count > 0)
                    {
                        SelectionStack.Pop();
                    }

                    if (SelectionStack.Count > 0)
                    {

                        var last = SelectionStack.Peek();
                        Debug.Log($"SelectionStack.Count:{SelectionStack.Count} last:{last}");
                        if (last != m_OdinMenuTree.Selection.SelectedValue && last != null)
                        {
                            TrySelectMenuItemWithObject(last);
                        }
                    }

                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("刷新编辑器缓存")))
                {
                    GameSupportEditorUtility.Refresh();
                    SelectionStack.Clear();
                }

            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }


        void InitOverviewWrapper<T, TOverview, TRowDetailWrapper, TTableRowWrapper, TNewRowWrapper, TRow>(OdinMenuTree tree, string menuMainKey, string menuDisplayName)
    where T : MetaTableOverviewWrapper<TOverview, TRowDetailWrapper, TTableRowWrapper, TNewRowWrapper, TRow>, new()
    where TOverview : MetaTableOverview
    where TRowDetailWrapper : MetaTableDetailRowWrapper<TOverview, TRow>, new()
    where TTableRowWrapper : MetaTableRowWrapper<TOverview, TNewRowWrapper, TRow>, new()
    where TNewRowWrapper : MetaTableNewRowWrapper<TOverview, TRow>, new()
    where TRow : MetaTableUnityRow, new()
        {
            var overviews = Pangoo.AssetDatabaseUtility.FindAsset<TOverview>().ToList();
            var overviewEditor = new T();
            overviewEditor.Overviews = overviews;
            overviewEditor.MenuWindow = this;
            // overviewEditor.MenuKey = menuMainKey;
            overviewEditor.MenuDisplayName = menuDisplayName;
            overviewEditor.Tree = tree;
            overviewEditor.Editor = this;
            overviewEditor.InitWrappers();
            tree.Add(menuDisplayName, overviewEditor);

        }

        OdinMenuTree m_OdinMenuTree;

        Stack<object> SelectionStack = new Stack<object>();

        public void OnSeletionChecned(SelectionChangedType changedType)
        {
            switch (changedType)
            {
                case SelectionChangedType.ItemAdded:
                    if (SelectionStack.Count == 0)
                    {
                        SelectionStack.Push(m_OdinMenuTree.Selection.SelectedValue);
                        return;
                    }

                    var target = SelectionStack.Peek();
                    if (target != m_OdinMenuTree.Selection.SelectedValue)
                    {
                        Debug.Log($"Push:{m_OdinMenuTree.Selection.SelectedValue} SelectionStack.Count:{SelectionStack.Count}");
                        SelectionStack.Push(m_OdinMenuTree.Selection.SelectedValue);
                        return;
                    }

                    break;
            }
        }

        public void ForceBuildMenuTree()
        {
            DetailWrapperDict.Clear();
            RowWrapperDict.Clear();
            m_OdinMenuTree = new OdinMenuTree(false);

            m_OdinMenuTree.Config.DrawSearchToolbar = true;
            m_OdinMenuTree.Config.AutoScrollOnSelectionChanged = true;
            m_OdinMenuTree.Selection.SelectionChanged += OnSeletionChecned;


            var config = Pangoo.AssetDatabaseUtility.FindAssetFirst<GameMainConfig>();
            if (config != null)
            {
                m_OdinMenuTree.Add("游戏配置", config);
            }

            InitOverviewWrapper<AssetGroupOverviewWrapper, AssetGroupOverview, AssetGroupDetailRowWrapper, AssetGroupRowWrapper, AssetGroupNewRowWrapper, UnityAssetGroupRow>(m_OdinMenuTree, null, "资源组");
            InitOverviewWrapper<AssetPathOverviewWrapper, Pangoo.MetaTable.AssetPathOverview, Pangoo.MetaTable.AssetPathDetailRowWrapper, Pangoo.MetaTable.AssetPathRowWrapper, Pangoo.MetaTable.AssetPathNewRowWrapper, UnityAssetPathRow>(m_OdinMenuTree, null, "资源路径");

            InitOverviewWrapper<SoundOverviewWrapper, Pangoo.MetaTable.SoundOverview, Pangoo.MetaTable.SoundDetailRowWrapper, Pangoo.MetaTable.SoundRowWrapper, Pangoo.MetaTable.SoundNewRowWrapper, UnitySoundRow>(m_OdinMenuTree, null, "音频");
            InitOverviewWrapper<SimpleUIOverviewWrapper, Pangoo.MetaTable.SimpleUIOverview, Pangoo.MetaTable.SimpleUIDetailRowWrapper, Pangoo.MetaTable.SimpleUIRowWrapper, Pangoo.MetaTable.SimpleUINewRowWrapper, UnitySimpleUIRow>(m_OdinMenuTree, null, "UI");
        }


        protected override OdinMenuTree BuildMenuTree()
        {
            if (m_OdinMenuTree == null)
            {
                ForceBuildMenuTree();
            }
            return m_OdinMenuTree;
        }

        Dictionary<string, object> DetailWrapperDict = new Dictionary<string, object>();

        Dictionary<string, object> RowWrapperDict = new Dictionary<string, object>();


        public void SetDetailWrapper(string uuid, object obj)
        {
            if (DetailWrapperDict.ContainsKey(uuid))
            {
                DetailWrapperDict[uuid] = obj;
            }
            else
            {
                DetailWrapperDict.Add(uuid, obj);
            }
        }

        public object GetDetailWrapper(string uuid)
        {
            if (DetailWrapperDict.TryGetValue(uuid, out object ret))
            {
                return ret;
            }
            return null;
        }

        public void SetRowWrapper(string uuid, object obj)
        {
            if (RowWrapperDict.ContainsKey(uuid))
            {
                RowWrapperDict[uuid] = obj;
            }
            else
            {
                RowWrapperDict.Add(uuid, obj);
            }
        }

        public object GetRowWrapper(string uuid)
        {
            if (RowWrapperDict.TryGetValue(uuid, out object ret))
            {
                return ret;
            }
            return null;
        }
    }

}