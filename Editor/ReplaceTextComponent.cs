using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
	public class ReplaceTextComponent : EditorWindow
	{
		private struct TMPTextData
		{
			public bool Enabled;
			public FontStyles FontStyle;
			public float FontSize;
			public float FontSizeMin;
			public float FontSizeMax;
			public float LineSpacing;
			public bool EnableRichText;
			public bool EnableAutoSizing;
			public TextAlignmentOptions TextAlignmentOptions;
			public bool WrappingEnabled;
			public TextOverflowModes TextOverflowModes;
			public string Text;
			public Color Color;
			public bool RayCastTarget;
		}
		
		[ContextMenu("Replace with Text-Mesh-Pro")]
		public static void Replace()
		{
			var selection = Selection.activeObject;
			Debug.Log(selection.name);
		}

		[MenuItem("CONTEXT/Text/To Text-Mesh-Pro")]
		public static void ReplaceMenu(MenuCommand command)
		{
			if (command.context is not Text { } component)
				return;

			var textData = GetTextData(component);
			var gameObject = component.gameObject;
			
			Undo.RecordObject(gameObject, "Text-To-TextMeshPro");
			Undo.DestroyObjectImmediate(component);
			CreateTMPText(textData, gameObject);

			
			Debug.Log($"[ {gameObject.name} ] converted to TextMeshProUGUI");
		}

		private static TextMeshProUGUI CreateTMPText(TMPTextData textData, GameObject target)
		{
			var tmpText = target.AddComponent<TextMeshProUGUI>();
			tmpText.enabled = textData.Enabled;
			tmpText.fontStyle = textData.FontStyle;
			tmpText.fontSize = textData.FontSize;
			tmpText.fontSizeMin = textData.FontSizeMin;
			tmpText.fontSizeMax = textData.FontSizeMax;
			tmpText.lineSpacing = textData.LineSpacing;
			tmpText.richText = textData.EnableRichText;
			tmpText.enableAutoSizing = textData.EnableAutoSizing;
			tmpText.alignment = textData.TextAlignmentOptions;
			tmpText.enableWordWrapping = textData.WrappingEnabled;
			tmpText.overflowMode = textData.TextOverflowModes;
			tmpText.text = textData.Text;
			tmpText.color = textData.Color;
			tmpText.raycastTarget = textData.RayCastTarget;
			return tmpText;
		}
		private static TMPTextData GetTextData(Text component)
			=> new ()
			{
				Enabled = component.enabled,
				FontStyle = FontStyleToFontStyles(component.fontStyle),
				FontSize = component.fontSize,
				FontSizeMin = component.resizeTextMinSize,
				FontSizeMax = component.resizeTextMaxSize,
				LineSpacing = component.lineSpacing,
				EnableRichText = component.supportRichText,
				EnableAutoSizing = component.resizeTextForBestFit,
				TextAlignmentOptions = TextAnchorToTextAlignmentOptions(component.alignment),
				WrappingEnabled = HorizontalWrapModeToBool(component.horizontalOverflow),
				TextOverflowModes = VerticalWrapModeToTextOverflowModes(component.verticalOverflow),
				Text = component.text,
				Color = component.color,
				RayCastTarget = component.raycastTarget
			};

		private static bool HorizontalWrapModeToBool(HorizontalWrapMode overflow) 
			=> overflow == HorizontalWrapMode.Wrap;

		private static TextOverflowModes VerticalWrapModeToTextOverflowModes(VerticalWrapMode verticalOverflow) 
			=> verticalOverflow == VerticalWrapMode.Truncate ? TextOverflowModes.Truncate : TextOverflowModes.Overflow;

		private static FontStyles FontStyleToFontStyles(FontStyle style)
		{
			return style switch
			{
				FontStyle.Normal => FontStyles.Normal,
				FontStyle.Bold => FontStyles.Bold,
				FontStyle.Italic => FontStyles.Italic,
				FontStyle.BoldAndItalic => FontStyles.Bold | FontStyles.Italic,
				_ => FontStyles.Normal
			};
		}

		private static TextAlignmentOptions TextAnchorToTextAlignmentOptions(TextAnchor anchor)
		{
			return anchor switch
			{
				TextAnchor.UpperLeft => TextAlignmentOptions.TopLeft,
				TextAnchor.UpperCenter => TextAlignmentOptions.Top,
				TextAnchor.UpperRight => TextAlignmentOptions.TopRight,
				TextAnchor.MiddleLeft => TextAlignmentOptions.Left,
				TextAnchor.MiddleCenter => TextAlignmentOptions.Center,
				TextAnchor.MiddleRight => TextAlignmentOptions.Right,
				TextAnchor.LowerLeft => TextAlignmentOptions.BottomLeft,
				TextAnchor.LowerCenter => TextAlignmentOptions.Bottom,
				TextAnchor.LowerRight => TextAlignmentOptions.BottomRight,
				_ => TextAlignmentOptions.TopLeft
			};
		}
	}
}
