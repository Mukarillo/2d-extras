﻿using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UnityEditor
{
    [CustomEditor(typeof(IsometricRuleTile), true)]
    [CanEditMultipleObjects]
    public class IsometricRuleTileEditor : RuleTileEditor
    {
        private static readonly int[, ] s_Arrows =
        {
            {3, 3, 0, 1, 1 },
            {6, 3, 0, 1, 2 },
            {6, 6, 9, 2, 2 },
            {6, 7, 8, 5, 2 },
            {7, 7, 8, 5, 5 },
        };

        internal override void RuleMatrixOnGUI(RuleTile ruleTile, Rect rect, RuleTile.TilingRule tilingRule)
        {
            Handles.color = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.2f) : new Color(0f, 0f, 0f, 0.2f);
            int index = 0;
            float w = rect.width / 5f;
            float h = rect.height / 5f;
            
            // Grid
            for (int y = 0; y <= 5; y++)
            {
                float left = rect.xMin + (y * rect.width) / 10;
                float right = left + rect.width / 2;
                float bottom = rect.yMin + (y * rect.height) / 10;
                float top = bottom + rect.height / 2;
                Handles.DrawLine(new Vector3(left, top), new Vector3(right, bottom));
            }
            for (int x = 0; x <= 5; x++)
            {
                float left = rect.xMin + (x * rect.width) / 10;
                float right = left + rect.width / 2;
                float top = rect.yMax - (x * rect.height) / 10;
                float bottom = top - rect.height / 2;
                Handles.DrawLine(new Vector3(left, bottom), new Vector3(right, top));
            }
            Handles.color = Color.white;

            // Icons
            for (int y = 0; y <= 4; y++)
            {
                for (int x = 0; x <= 4; x++)
                {
                    Rect r = new Rect(
                        rect.xMin + ((x + y) * rect.width) / 6, 
                        rect.yMin + ((2 - x + y) * rect.height) / 6, 
                        w - 1, h - 1);
                    if (x != 2 || y != 2)
                    {
                        RuleOnGUI(r, s_Arrows[y, x], tilingRule.m_Neighbors[index]);
                        RuleNeighborUpdate(r, tilingRule, index);

                        index++;
                    }
                    else
                    {
                        RuleTransformOnGUI(r, tilingRule.m_RuleTransform);
                        RuleTransformUpdate(r, tilingRule);
                    }
                }
            }
        }

        internal override bool ContainsMousePosition(Rect rect)
        {
            var center = rect.center;
            var halfWidth = rect.width / 2;
            var halfHeight = rect.height / 2;
            var mouseFromCenter = Event.current.mousePosition - center;
            var xAbs = Mathf.Abs(Vector2.Dot(mouseFromCenter, Vector2.right));
            var yAbs = Mathf.Abs(Vector2.Dot(mouseFromCenter, Vector2.up));
            return (xAbs / halfWidth + yAbs / halfHeight) <= 1;
        }
    }
}
