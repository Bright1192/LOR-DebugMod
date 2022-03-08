using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*==========================
* 创建时间: 2022/3/8 23:44:33
* 作者: Bright
*==========================*/
namespace UnityExplorer_hp
{
    public static class MyUITool
    {

        public static GameObject CreateUIObject(string name, GameObject parent, Vector2 size = default(Vector2))
        {
            GameObject gameObject = new GameObject(name)
            {
                layer = 5,
                hideFlags = HideFlags.HideAndDontSave
            };
            bool flag2 = parent;
            if (flag2)
            {
                gameObject.transform.SetParent(parent.transform, false);
            }
            RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
            rectTransform.sizeDelta = size;
            return gameObject;
        }
        public static GameObject CanvasRoot { get; private set; }
        private static void CreateRootCanvas()
        {
            CanvasRoot = new GameObject("UniverseLibCanvas");
            CanvasRoot.AddComponent<BaseUI>();
            UnityEngine.Object.DontDestroyOnLoad(CanvasRoot);
            CanvasRoot.hideFlags |= HideFlags.HideAndDontSave;
            CanvasRoot.layer = 5;
            CanvasRoot.transform.position = new Vector3(0f, 0f, 1f);
            CanvasRoot.SetActive(false);
            CanvasRoot.AddComponent<EventSystem>().enabled = false;
            CanvasRoot.SetActive(true);
        }
        static MyUITool()
        {
            CreateRootCanvas();

        }
        public static GameObject RegisterUI(string id, Action updateMethod)
        {
            GameObject gameObject = CreateUIObject(id + "_Root", CanvasRoot, default(Vector2));
            gameObject.SetActive(false);
            Canvas canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.referencePixelsPerUnit = 100f;
            canvas.sortingOrder = 999;
            CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>();
            canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            gameObject.AddComponent<GraphicRaycaster>();
            RectTransform component = gameObject.GetComponent<RectTransform>();
            component.anchorMin = Vector2.zero;
            component.anchorMax = Vector2.one;
            component.pivot = new Vector2(0.5f, 0.5f);
            if (updateMethod != null)
            {
                BaseUI.Updates[id] = updateMethod;
            }
            gameObject.SetActive(true);
            gameObject.transform.SetParent(CanvasRoot.transform, false);
            return gameObject;
        }
        /// <summary>
        /// Get and/or Add a LayoutElement component to the GameObject, and set any of the values on it.
        /// </summary>
        public static LayoutElement SetLayoutElement(GameObject gameObject, int? minWidth = null, int? minHeight = null, int? flexibleWidth = null, int? flexibleHeight = null, int? preferredWidth = null, int? preferredHeight = null, bool? ignoreLayout = null)
        {
            LayoutElement layoutElement = gameObject.GetComponent<LayoutElement>();
            bool flag = !layoutElement;
            if (flag)
            {
                layoutElement = gameObject.AddComponent<LayoutElement>();
            }
            bool flag2 = minWidth != null;
            if (flag2)
            {
                layoutElement.minWidth = minWidth.Value;
            }
            bool flag3 = minHeight != null;
            if (flag3)
            {
                layoutElement.minHeight = minHeight.Value;
            }
            bool flag4 = flexibleWidth != null;
            if (flag4)
            {
                layoutElement.flexibleWidth = flexibleWidth.Value;
            }
            bool flag5 = flexibleHeight != null;
            if (flag5)
            {
                layoutElement.flexibleHeight = flexibleHeight.Value;
            }
            bool flag6 = preferredWidth != null;
            if (flag6)
            {
                layoutElement.preferredWidth = preferredWidth.Value;
            }
            bool flag7 = preferredHeight != null;
            if (flag7)
            {
                layoutElement.preferredHeight = preferredHeight.Value;
            }
            bool flag8 = ignoreLayout != null;
            if (flag8)
            {
                layoutElement.ignoreLayout = ignoreLayout.Value;
            }
            return layoutElement;
        }
        /// <summary>
        /// Get and/or Add a HorizontalOrVerticalLayoutGroup (must pick one) to the GameObject, and set the values on it.
        /// </summary>
        // Token: 0x060000EA RID: 234 RVA: 0x00006810 File Offset: 0x00004A10
        public static T SetLayoutGroup<T>(GameObject gameObject, bool? forceWidth = null, bool? forceHeight = null, bool? childControlWidth = null, bool? childControlHeight = null, int? spacing = null, int? padTop = null, int? padBottom = null, int? padLeft = null, int? padRight = null, TextAnchor? childAlignment = null) where T : HorizontalOrVerticalLayoutGroup
        {
            T t = gameObject.GetComponent<T>();
            bool flag = !t;
            if (flag)
            {
                t = gameObject.AddComponent<T>();
            }
            return SetLayoutGroup<T>(t, forceWidth, forceHeight, childControlWidth, childControlHeight, spacing, padTop, padBottom, padLeft, padRight, childAlignment);
        }

        /// <summary>
        /// Set the values on a HorizontalOrVerticalLayoutGroup.
        /// </summary>
        // Token: 0x060000EB RID: 235 RVA: 0x0000685C File Offset: 0x00004A5C
        public static T SetLayoutGroup<T>(T group, bool? forceWidth = null, bool? forceHeight = null, bool? childControlWidth = null, bool? childControlHeight = null, int? spacing = null, int? padTop = null, int? padBottom = null, int? padLeft = null, int? padRight = null, TextAnchor? childAlignment = null) where T : HorizontalOrVerticalLayoutGroup
        {
            bool flag = forceWidth != null;
            if (flag)
            {
                group.childForceExpandWidth = forceWidth.Value;
            }
            bool flag2 = forceHeight != null;
            if (flag2)
            {
                group.childForceExpandHeight = forceHeight.Value;
            }
            bool flag3 = childControlWidth != null;
            if (flag3)
            {
                group.childControlWidth = childControlWidth.Value;
            }
            bool flag4 = childControlHeight != null;
            if (flag4)
            {
                group.childControlHeight = childControlHeight.Value;
            }
            bool flag5 = spacing != null;
            if (flag5)
            {
                group.spacing = spacing.Value;
            }
            bool flag6 = padTop != null;
            if (flag6)
            {
                group.padding.top = padTop.Value;
            }
            bool flag7 = padBottom != null;
            if (flag7)
            {
                group.padding.bottom = padBottom.Value;
            }
            bool flag8 = padLeft != null;
            if (flag8)
            {
                group.padding.left = padLeft.Value;
            }
            bool flag9 = padRight != null;
            if (flag9)
            {
                group.padding.right = padRight.Value;
            }
            bool flag10 = childAlignment != null;
            if (flag10)
            {
                group.childAlignment = childAlignment.Value;
            }
            return group;
        }
        public static void SetColorBlock(Selectable selectable, Color? normal = null, Color? highlighted = null, Color? pressed = null, Color? disabled = null)
        {
            ColorBlock colors = selectable.colors;
            bool flag = normal != null;
            if (flag)
            {
                colors.normalColor = normal.Value;
            }
            bool flag2 = highlighted != null;
            if (flag2)
            {
                colors.highlightedColor = highlighted.Value;
            }
            bool flag3 = pressed != null;
            if (flag3)
            {
                colors.pressedColor = pressed.Value;
            }
            bool flag4 = disabled != null;
            if (flag4)
            {
                colors.disabledColor = disabled.Value;
            }
            selectable.colors = colors;
        }
        // Token: 0x060000E7 RID: 231 RVA: 0x00006671 File Offset: 0x00004871
        public static void SetDefaultTextValues(Text text)
        {
            text.color = Color.white;
            text.font = DefaultFont;
            text.fontSize = 14;
        }

        public  static Vector2 smallElementSize = new Vector2(25f, 25f);
        public static readonly Font DefaultFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
        /// <summary>
        /// 创建一个输入框
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="placeHolderText"></param>
        /// <returns></returns>
        public static InputField CreateInputField(GameObject parent, string name = "InputField", string placeHolderText = "...")
        {
            GameObject gameObject = CreateUIObject(name, parent, default(Vector2));
            Image image = gameObject.AddComponent<Image>();
            image.type = Image.Type.Sliced;
            image.color = new Color(0f, 0f, 0f, 0.5f);
            InputField inputField = gameObject.AddComponent<InputField>();
            Navigation navigation = inputField.navigation;
            navigation.mode = Navigation.Mode.None;
            inputField.navigation = navigation;
            inputField.lineType = InputField.LineType.SingleLine;
            inputField.interactable = true;
            inputField.transition = Selectable.Transition.ColorTint;
            inputField.targetGraphic = image;
            SetColorBlock(inputField, new Color?(new Color(1f, 1f, 1f, 1f)), new Color?(new Color(0.95f, 0.95f, 0.95f, 1f)), new Color?(new Color(0.78f, 0.78f, 0.78f, 1f)), null);
            GameObject gameObject2 = CreateUIObject("TextArea", gameObject, default(Vector2));
            gameObject2.AddComponent<RectMask2D>();
            RectTransform component = gameObject2.GetComponent<RectTransform>();
            component.anchorMin = Vector2.zero;
            component.anchorMax = Vector2.one;
            component.offsetMin = Vector2.zero;
            component.offsetMax = Vector2.zero;
            GameObject gameObject3 = CreateUIObject("Placeholder", gameObject2, default(Vector2));
            Text text = gameObject3.AddComponent<Text>();
            SetDefaultTextValues(text);
            text.text = (placeHolderText ?? "...");
            text.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.alignment = TextAnchor.MiddleLeft;
            text.fontSize = 14;
            RectTransform component2 = gameObject3.GetComponent<RectTransform>();
            component2.anchorMin = Vector2.zero;
            component2.anchorMax = Vector2.one;
            component2.offsetMin = Vector2.zero;
            component2.offsetMax = Vector2.zero;
            inputField.placeholder = text;
            GameObject gameObject4 = CreateUIObject("Text", gameObject2, default(Vector2));
            Text text2 = gameObject4.AddComponent<Text>();
            SetDefaultTextValues(text2);
            text2.text = "";
            text2.color = new Color(1f, 1f, 1f, 1f);
            text2.horizontalOverflow = HorizontalWrapMode.Wrap;
            text2.alignment = TextAnchor.MiddleLeft;
            text2.fontSize = 14;
            RectTransform component3 = gameObject4.GetComponent<RectTransform>();
            component3.anchorMin = Vector2.zero;
            component3.anchorMax = Vector2.one;
            component3.offsetMin = Vector2.zero;
            component3.offsetMax = Vector2.zero;
            inputField.textComponent = text2;
            inputField.characterLimit = 16000;
            var inputFieldRef = inputField;
            ContentSizeFitter contentSizeFitter = inputFieldRef.gameObject.AddComponent<ContentSizeFitter>();
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            inputFieldRef.lineType = InputField.LineType.MultiLineNewline;
            int? num2 = new int?(25);
            int? minWidth2 = new int?(200);
            int? minHeight2 = num2;
            int? num = null;
            int? flexibleWidth2 = num;
            num = null;
            int? flexibleHeight2 = num;
            num = null;
            int? preferredWidth2 = num;
            num = null;
            SetLayoutElement(inputFieldRef.gameObject, minWidth2, minHeight2, flexibleWidth2, flexibleHeight2, preferredWidth2, num, null);
            return inputFieldRef;
        }
        public static readonly Color disabledButtonColor = new Color(0.25f, 0.25f, 0.25f);
        /// <summary>
        /// 创建一个按钮
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="OnClick"></param>
        /// <param name="normalColor"></param>
        /// <returns></returns>
        public static Button CreateButton(GameObject parent, string name, string text, Action OnClick = null, Color? normalColor = null)
        {
            ColorBlock colors = default(ColorBlock);
            normalColor = new Color?(normalColor ?? new Color(0.25f, 0.25f, 0.25f));

            GameObject gameObject = CreateUIObject(name, parent, new Vector2(25f, 25f));
            GameObject gameObject2 = CreateUIObject("Text", gameObject, default(Vector2));
            Image image = gameObject.AddComponent<Image>();
            image.type = Image.Type.Sliced;
            image.color = new Color(1f, 1f, 1f, 1f);
            Button button = gameObject.AddComponent<Button>();
            Navigation navigation = button.navigation;
            navigation.mode = Navigation.Mode.Explicit;
            button.navigation = navigation;
            SetColorBlock(button, new Color?(new Color(0.2f, 0.2f, 0.2f)), new Color?(new Color(0.3f, 0.3f, 0.3f)), new Color?(new Color(0.15f, 0.15f, 0.15f)), null);
            colors.colorMultiplier = 1f;
            button.colors = colors;
            Text text2 = gameObject2.AddComponent<Text>();
            text2.text = text;
            SetDefaultTextValues(text2);
            text2.alignment = TextAnchor.MiddleCenter;
            RectTransform component = gameObject2.GetComponent<RectTransform>();
            component.anchorMin = Vector2.zero;
            component.anchorMax = Vector2.one;
            component.sizeDelta = Vector2.zero;


            SetColorBlock(button, normalColor, normalColor * 1.2f, normalColor * 0.7f, null);





            var NavButton = button;
            gameObject = NavButton.gameObject;
            gameObject.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            SetLayoutGroup<HorizontalLayoutGroup>(gameObject, new bool?(false), new bool?(true), new bool?(true), new bool?(true), new int?(0), new int?(0), new int?(0), new int?(5), new int?(5), new TextAnchor?(TextAnchor.MiddleCenter));
            gameObject2 = gameObject;
            int? minWidth = new int?(80);
            int? num = null;
            int? minHeight = num;
            num = null;
            int? flexibleWidth = num;
            num = null;
            int? flexibleHeight = num;
            num = null;
            int? preferredWidth = num;
            num = null;
            SetLayoutElement(gameObject2, minWidth, minHeight, flexibleWidth, flexibleHeight, preferredWidth, num, null);
            SetColorBlock(NavButton, new Color?(disabledButtonColor), new Color?(disabledButtonColor * 1.2f), null, null);
            NavButton.onClick.RemoveAllListeners();
            if (OnClick != null)
            {
                NavButton.onClick.AddListener(delegate () { OnClick(); });
            }
            GameObject gameObject3 = gameObject.transform.Find("Text").gameObject;
            gameObject3.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            return NavButton;
        }
        public static GameObject CreateScrollbar(GameObject parent, string name, out Scrollbar scrollbar)
        {
            GameObject gameObject = CreateUIObject(name, parent, new Vector2(25f, 25f));
            GameObject gameObject2 = CreateUIObject("Sliding Area", gameObject, default(Vector2));
            GameObject gameObject3 = CreateUIObject("Handle", gameObject2, default(Vector2));
            Image image = gameObject.AddComponent<Image>();
            image.type = Image.Type.Sliced;
            image.color = new Color(0.1f, 0.1f, 0.1f);
            Image image2 = gameObject3.AddComponent<Image>();
            image2.type = Image.Type.Sliced;
            image2.color = new Color(0.4f, 0.4f, 0.4f);
            RectTransform component = gameObject2.GetComponent<RectTransform>();
            component.sizeDelta = new Vector2(-20f, -20f);
            component.anchorMin = Vector2.zero;
            component.anchorMax = Vector2.one;
            RectTransform component2 = gameObject3.GetComponent<RectTransform>();
            component2.sizeDelta = new Vector2(20f, 20f);
            scrollbar = gameObject.AddComponent<Scrollbar>();
            scrollbar.handleRect = component2;
            scrollbar.targetGraphic = image2;
            Navigation navigation = scrollbar.navigation;
            navigation.mode = Navigation.Mode.Explicit;
            scrollbar.navigation = navigation;
            SetColorBlock(scrollbar, new Color?(new Color(0.2f, 0.2f, 0.2f)), new Color?(new Color(0.3f, 0.3f, 0.3f)), new Color?(new Color(0.15f, 0.15f, 0.15f)), null);
            return gameObject;
        }
        public static void AddListener<T>(this UnityEvent<T> _event, Action<T> listener)
        {
            _event.AddListener(new UnityAction<T>(listener.Invoke));
        }
        /// <summary>
        /// 创建一个下拉选择菜单
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="defaultItemText"></param>
        /// <param name="itemFontSize"></param>
        /// <param name="onValueChanged"></param>
        /// <param name="defaultOptions"></param>
        /// <returns></returns>
        public static Dropdown CreateDropdown(GameObject parent, string defaultItemText, int itemFontSize, Action<int> onValueChanged, string[] defaultOptions = null)
        {
            Dropdown dropdown;
            GameObject gameObject = CreateUIObject("Dropdown", parent, new Vector2(100f, 30f));
            GameObject gameObject2 = CreateUIObject("Label", gameObject, default(Vector2));
            GameObject gameObject3 = CreateUIObject("Arrow", gameObject, default(Vector2));
            GameObject gameObject4 = CreateUIObject("Template", gameObject, default(Vector2));
            GameObject gameObject5 = CreateUIObject("Viewport", gameObject4, default(Vector2));
            GameObject gameObject6 = CreateUIObject("Content", gameObject5, default(Vector2));
            GameObject gameObject7 = CreateUIObject("Item", gameObject6, default(Vector2));
            GameObject gameObject8 = CreateUIObject("Item Background", gameObject7, default(Vector2));
            GameObject gameObject9 = CreateUIObject("Item Checkmark", gameObject7, default(Vector2));
            GameObject gameObject10 = CreateUIObject("Item Label", gameObject7, default(Vector2));
            Scrollbar scrollbar;
            GameObject gameObject11 = CreateScrollbar(gameObject4, "DropdownScroll", out scrollbar);
            scrollbar.SetDirection(Scrollbar.Direction.BottomToTop, true);
            SetColorBlock(scrollbar, new Color?(new Color(0.45f, 0.45f, 0.45f)), new Color?(new Color(0.6f, 0.6f, 0.6f)), new Color?(new Color(0.4f, 0.4f, 0.4f)), null);
            RectTransform component = gameObject11.GetComponent<RectTransform>();
            component.anchorMin = Vector2.right;
            component.anchorMax = Vector2.one;
            component.pivot = Vector2.one;
            component.sizeDelta = new Vector2(component.sizeDelta.x, 0f);
            Text text = gameObject10.AddComponent<Text>();
            SetDefaultTextValues(text);
            text.alignment = TextAnchor.MiddleLeft;
            text.text = defaultItemText;
            text.fontSize = itemFontSize;
            Text text2 = gameObject3.AddComponent<Text>();
            SetDefaultTextValues(text2);
            text2.text = "▼";
            RectTransform component2 = gameObject3.GetComponent<RectTransform>();
            component2.anchorMin = new Vector2(1f, 0.5f);
            component2.anchorMax = new Vector2(1f, 0.5f);
            component2.sizeDelta = new Vector2(20f, 20f);
            component2.anchoredPosition = new Vector2(-15f, 0f);
            Image image = gameObject8.AddComponent<Image>();
            image.color = new Color(0.25f, 0.35f, 0.25f, 1f);
            Toggle itemToggle = gameObject7.AddComponent<Toggle>();
            itemToggle.targetGraphic = image;
            itemToggle.isOn = true;
            SetColorBlock(itemToggle, new Color?(new Color(0.35f, 0.35f, 0.35f, 1f)), new Color?(new Color(0.25f, 0.55f, 0.25f, 1f)), null, null);
            itemToggle.onValueChanged.AddListener(delegate (bool val)
            {
                itemToggle.OnDeselect(null);
            });
            Image image2 = gameObject4.AddComponent<Image>();
            image2.type = Image.Type.Sliced;
            image2.color = Color.black;
            ScrollRect scrollRect = gameObject4.AddComponent<ScrollRect>();
            scrollRect.scrollSensitivity = 35f;
            scrollRect.content = gameObject6.GetComponent<RectTransform>();
            scrollRect.viewport = gameObject5.GetComponent<RectTransform>();
            scrollRect.horizontal = false;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.verticalScrollbar = scrollbar;
            scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
            scrollRect.verticalScrollbarSpacing = -3f;
            gameObject5.AddComponent<Mask>().showMaskGraphic = false;
            Image image3 = gameObject5.AddComponent<Image>();
            image3.type = Image.Type.Sliced;
            Text text3 = gameObject2.AddComponent<Text>();
            SetDefaultTextValues(text3);
            text3.alignment = TextAnchor.MiddleLeft;
            Image image4 = gameObject.AddComponent<Image>();
            image4.color = new Color(0.04f, 0.04f, 0.04f, 0.75f);
            image4.type = Image.Type.Sliced;
            dropdown = gameObject.AddComponent<Dropdown>();
            dropdown.targetGraphic = image4;
            dropdown.template = gameObject4.GetComponent<RectTransform>();
            dropdown.captionText = text3;
            dropdown.itemText = text;
            dropdown.RefreshShownValue();
            RectTransform component3 = gameObject2.GetComponent<RectTransform>();
            component3.anchorMin = Vector2.zero;
            component3.anchorMax = Vector2.one;
            component3.offsetMin = new Vector2(10f, 2f);
            component3.offsetMax = new Vector2(-28f, -2f);
            RectTransform component4 = gameObject4.GetComponent<RectTransform>();
            component4.anchorMin = new Vector2(0f, 0f);
            component4.anchorMax = new Vector2(1f, 0f);
            component4.pivot = new Vector2(0.5f, 1f);
            component4.anchoredPosition = new Vector2(0f, 2f);
            component4.sizeDelta = new Vector2(0f, 150f);
            RectTransform component5 = gameObject5.GetComponent<RectTransform>();
            component5.anchorMin = new Vector2(0f, 0f);
            component5.anchorMax = new Vector2(1f, 1f);
            component5.sizeDelta = new Vector2(-18f, 0f);
            component5.pivot = new Vector2(0f, 1f);
            RectTransform component6 = gameObject6.GetComponent<RectTransform>();
            component6.anchorMin = new Vector2(0f, 1f);
            component6.anchorMax = new Vector2(1f, 1f);
            component6.pivot = new Vector2(0.5f, 1f);
            component6.anchoredPosition = new Vector2(0f, 0f);
            component6.sizeDelta = new Vector2(0f, 28f);
            RectTransform component7 = gameObject7.GetComponent<RectTransform>();
            component7.anchorMin = new Vector2(0f, 0.5f);
            component7.anchorMax = new Vector2(1f, 0.5f);
            component7.sizeDelta = new Vector2(0f, 25f);
            RectTransform component8 = gameObject8.GetComponent<RectTransform>();
            component8.anchorMin = Vector2.zero;
            component8.anchorMax = Vector2.one;
            component8.sizeDelta = Vector2.zero;
            RectTransform component9 = gameObject10.GetComponent<RectTransform>();
            component9.anchorMin = Vector2.zero;
            component9.anchorMax = Vector2.one;
            component9.offsetMin = new Vector2(20f, 1f);
            component9.offsetMax = new Vector2(-10f, -2f);
            gameObject4.SetActive(false);
            bool flag = onValueChanged != null;
            if (flag)
            {
                dropdown.onValueChanged.AddListener(onValueChanged);
            }
            bool flag2 = defaultOptions != null;
            if (flag2)
            {
                foreach (string text4 in defaultOptions)
                {
                    dropdown.options.Add(new Dropdown.OptionData(text4));
                }
            }

            int? num = new int?(25);
            int? minWidth18 = new int?(110);
            int? minHeight18 = num;
            int? flexibleWidth18 = new int?(999);
            int? num2 = null;
            int? flexibleHeight18 = num2;
            num2 = null;
            int? preferredWidth18 = num2;
            num2 = null;
            SetLayoutElement(gameObject, minWidth18, minHeight18, flexibleWidth18, flexibleHeight18, preferredWidth18, num2, null);
            dropdown.captionText.color = new Color(0.57f, 0.76f, 0.43f);
            dropdown.value = 0;
            dropdown.RefreshShownValue();
            return dropdown;
        }
        public static void SetDefaultSelectableValues(Selectable selectable)
        {
            Navigation navigation = selectable.navigation;
            navigation.mode = Navigation.Mode.Explicit;
            selectable.navigation = navigation;
            SetColorBlock(selectable, new Color?(new Color(0.2f, 0.2f, 0.2f)), new Color?(new Color(0.3f, 0.3f, 0.3f)), new Color?(new Color(0.15f, 0.15f, 0.15f)), null);
        }
        /// <summary>
        /// Create a Toggle control component.
        /// </summary>
        /// <param name="parent">The parent object to build onto</param>
        /// <param name="name">The GameObject name of your toggle</param>
        /// <param name="toggle">Returns the created Toggle component</param>
        /// <param name="text">Returns the Text component for your Toggle</param>
        /// <param name="bgColor">The background color of the checkbox</param>
        /// <param name="checkWidth">The width of your checkbox</param>
        /// <param name="checkHeight">The height of your checkbox</param>
        /// <returns>The root GameObject for your Toggle control</returns>
        // Token: 0x060000D8 RID: 216 RVA: 0x00006E3C File Offset: 0x0000503C
        public static GameObject CreateToggle(GameObject parent, string name, out Toggle toggle, out Text text, Color bgColor = default(Color), int checkWidth = 20, int checkHeight = 20)
        {
            GameObject gameObject = CreateUIObject(name, parent, new Vector2(25f, 25f));
            SetLayoutGroup<HorizontalLayoutGroup>(gameObject, new bool?(false), new bool?(false), new bool?(true), new bool?(true), new int?(5), new int?(0), new int?(0), new int?(0), new int?(0), new TextAnchor?(TextAnchor.MiddleLeft));
            toggle = gameObject.AddComponent<Toggle>();
            toggle.isOn = true;
            SetDefaultSelectableValues(toggle);
            Toggle t2 = toggle;
            toggle.onValueChanged.AddListener(delegate (bool _)
            {
                t2.OnDeselect(null);
            });
            GameObject gameObject2 = CreateUIObject("Background", gameObject, default(Vector2));
            Image image = gameObject2.AddComponent<Image>();
            image.color = ((bgColor == default(Color)) ? new Color(0.04f, 0.04f, 0.04f, 0.75f) : bgColor);
            SetLayoutGroup<HorizontalLayoutGroup>(gameObject2, new bool?(true), new bool?(true), new bool?(true), new bool?(true), new int?(0), new int?(2), new int?(2), new int?(2), new int?(2), null);
            GameObject gameObject3 = gameObject2;
            int? minWidth = new int?(checkWidth);
            int? flexibleWidth = new int?(0);
            SetLayoutElement(gameObject3, minWidth, new int?(checkHeight), flexibleWidth, new int?(0), null, null, null);
            GameObject gameObject4 = CreateUIObject("Checkmark", gameObject2, default(Vector2));
            Image image2 = gameObject4.AddComponent<Image>();
            image2.color = new Color(0.8f, 1f, 0.8f, 0.3f);
            GameObject gameObject5 = CreateUIObject("Label", gameObject, default(Vector2));
            text = gameObject5.AddComponent<Text>();
            text.text = "";
            text.alignment = TextAnchor.MiddleLeft;
            SetDefaultTextValues(text);
            GameObject gameObject6 = gameObject5;
            int? minWidth2 = new int?(0);
            flexibleWidth = new int?(0);
            SetLayoutElement(gameObject6, minWidth2, new int?(checkHeight), flexibleWidth, new int?(0), null, null, null);
            toggle.graphic = image2;
            toggle.targetGraphic = image;
            SetLayoutElement(gameObject, new int?(17), new int?(17), null, new int?(0), null, null, null);
            return gameObject;
        }
    }
    public class BaseUI : MonoBehaviour
    {
        public static Dictionary<string, Action> Updates = new Dictionary<string, Action>();
        public void Update()
        {
            foreach (var x in Updates.Values)
            {
                x();
            }
        }
    }
}
