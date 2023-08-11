using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Nefta.ToolboxDemo.Test
{
    public class IntegrationTest
    {
        [UnityTest]
        public IEnumerator GuestSignUpTest()
        {
            SceneManager.LoadScene("ToolboxDemoScene", LoadSceneMode.Single);
            while (SceneManager.GetActiveScene().name != "ToolboxDemoScene")
            {
                yield return null;
            }
            Assert.IsTrue(SceneManager.GetActiveScene().name == "ToolboxDemoScene");

            var eventSystem = GetObject<EventSystem>("EventSystem");
            
            var guestSignUpButton = GetObject<Button>("GuestSignUp_Button");
            guestSignUpButton.OnPointerClick(new PointerEventData(eventSystem));

            var nameInput = GetObject<InputField>("GuestPanel/Name_InputField");
            var username = "testUser_" + System.Guid.NewGuid();
            nameInput.text = username;
            guestSignUpButton = GetObject<Button>("GuestPanel/SignUpGuest_Button");
            guestSignUpButton.OnPointerClick(new PointerEventData(eventSystem));

            var userPanel = GetObject<GameObject>("UserPanel");
            while (!userPanel.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    yield break;
                }
                yield return null;
            }

            var userNameField = GameObject.Find("Username_Text").GetComponent<Text>();
            Assert.AreEqual(username, userNameField.text.Substring(userNameField.text.IndexOf(' ') + 1));
        }

        private T GetObject<T>(string name) where T : Object
        {
            var path = name.Split('/');
#if UNITY_2022_3_OR_NEWER
            var unityObjects = Object.FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);
#else
            var unityObjects = Object.FindObjectsOfType<T>(true);
#endif
            foreach (var unityObject in unityObjects)
            {
                if (unityObject.name == path[path.Length - 1])
                {
                    if (path.Length > 1)
                    {
                        var component = (Component)(Object)unityObject;
                        if (component.transform.parent.name == path[path.Length - 2])
                        {
                            return unityObject;
                        }
                    }
                    else
                    {
                        return unityObject;
                    }
                }
            }

            return null;
        }
    }
}
