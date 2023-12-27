using System;
using System.Collections.Generic;
using Nefta.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace Nefta.ToolboxSdk.Editor
{
    public class NeftaToolboxEditor : INeftaEditorModule
    {
        private const string MetaMaskIntegrationSymbol = "NEFTA_INTEGRATION_METAMASK";
        
        private ToolboxConfiguration _configuration;
        
        private bool _isMetaMaskIntegration;
        private GUIStyle _multiLineLabelStyle;
        
        [InitializeOnLoadMethod]
        private static void Init()
        {
            if (EditorApplication.isUpdating)
            {
                EditorApplication.delayCall += Init;
                return;
            }
            
            var instance = new NeftaToolboxEditor();
            NeftaEditorWindow.RegisterModule(instance);
            var coreConfiguration = NeftaEditorWindow.GetConfiguration();
            if (coreConfiguration == null)
            {
                return;
            }

            foreach (var configuration in coreConfiguration._configurations)
            {
                if (configuration is ToolboxConfiguration)
                {
                    instance._configuration = (ToolboxConfiguration) configuration;
                }
            }
            
            if (instance._configuration == null)
            {
                instance._configuration = new ToolboxConfiguration();
                coreConfiguration._configurations.Add(instance._configuration);
                
                instance._configuration._preloadStrategy = Toolbox.PreloadStrategies.Full;
            }
        }

        public void AddPages(Dictionary<string, Action> pages)
        {
            pages.Add("Toolbox", OnToolboxPage);
            
            _multiLineLabelStyle = EditorStyles.label;
            _multiLineLabelStyle.wordWrap = true;
            
            _isMetaMaskIntegration = NeftaEditorWindow.GetDefines().Contains(MetaMaskIntegrationSymbol);
        }
        
        private void OnToolboxPage()
        {
            if (String.IsNullOrEmpty(_configuration._marketplaceId))
            {
                GUILayout.Label("To get started, configure your marketplaceId (starting with \"m-\") when initializing the Toolbox:",
                    _multiLineLabelStyle);
            }
            
            EditorGUILayout.BeginHorizontal();
            _configuration._marketplaceId = EditorGUILayout.TextField("Marketplace ID", _configuration._marketplaceId);
            if (GUILayout.Button("Apply"))
            {
                NeftaEditorWindow.UpdateConfigurationOnDisk();

            }
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Space(20);
            
            var isMetaMaskIntegration = GUILayout.Toggle(_isMetaMaskIntegration, "Enable MetaMask integration");
            if (isMetaMaskIntegration != _isMetaMaskIntegration)
            {
                _isMetaMaskIntegration = isMetaMaskIntegration;
                NeftaEditorWindow.SetSymbolEnabled(MetaMaskIntegrationSymbol, _isMetaMaskIntegration);
            }
            
            GUILayout.Space(20);
            
            var preloadStrategy = (Toolbox.PreloadStrategies) EditorGUILayout.EnumPopup("Preload strategy", _configuration._preloadStrategy);
            if (preloadStrategy != _configuration._preloadStrategy)
            {
                _configuration._preloadStrategy = preloadStrategy;
                
                NeftaEditorWindow.UpdateConfigurationOnDisk();
            }
            
            GUILayout.Space(20);
            
            if (GUILayout.Button("Clear Asset cache"))
            {
                Toolbox.ClearCache();
                
                Debug.Log("Asset cache cleared");
            }
        }
    }   
}
