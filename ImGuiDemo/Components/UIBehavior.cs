using Evergine.Bindings.Imgui;
using Evergine.Bindings.Imguizmo;
using Evergine.Bindings.Imnodes;
using Evergine.Bindings.Implot;
using Evergine.Framework;
using Evergine.Mathematics;
using Evergine.UI;
using System;

namespace ImGuiDemo.Components
{
    public unsafe class UIBehavior : Behavior
    {
        private bool imguiDemoOpen = true;
        private bool implotDemoOpen = true;
        private bool imnodesDemoOpen = true;

        protected override void Update(TimeSpan gameTime)
        {
            // Imgui
            if (this.imguiDemoOpen)
            {
                ImguiNative.igShowDemoWindow(this.imguiDemoOpen.Pointer());
            }

            // Implot
            if (this.implotDemoOpen)
            {
                ImplotNative.ImPlot_ShowDemoWindow(this.implotDemoOpen.Pointer());
            }

            // Imguizmo
            var io = ImguiNative.igGetIO_Nil();
            ImguizmoNative.ImGuizmo_SetRect(0, 0, io->DisplaySize.X, io->DisplaySize.Y);

            var camera = this.Managers.RenderManager.ActiveCamera3D;
            Matrix4x4 view = camera.View;
            Matrix4x4 project = camera.Projection;

            ImguizmoNative.ImGuizmo_ViewManipulate_Float(view.Pointer(), 2, Vector2.Zero, new Vector2(128, 128), 0x10101010);

            Matrix4x4.Invert(ref view, out Matrix4x4 iview);
            var translation = iview.Translation;
            var rotation = iview.Rotation;

            Vector3* r = &rotation;
            camera.Transform.LocalRotation = *r;

            Vector3* t = &translation;
            camera.Transform.LocalPosition = *t;

            // Imnodes
            if (this.imnodesDemoOpen)
            {
                ImguiNative.igSetNextWindowSize(new Vector2(500, 500), ImGuiCond.Appearing);
                ImguiNative.igBegin("ImNodes Demo", this.imnodesDemoOpen.Pointer(), ImGuiWindowFlags.None);

                string[] nodes = new string[] { "Node1", "Node2", "Node3" };
                ImnodesNative.imnodes_BeginNodeEditor();
                int id = 0;
                for (int i = 0; i < nodes.Length; i++)
                {
                    var node = nodes[i];

                    ImnodesNative.imnodes_BeginNode(id++);

                    ImnodesNative.imnodes_BeginNodeTitleBar();
                    ImguiNative.igText(node);
                    ImnodesNative.imnodes_EndNodeTitleBar();

                    ImnodesNative.imnodes_BeginInputAttribute(id++, ImNodesPinShape.Circle);
                    ImguiNative.igText("input");
                    ImnodesNative.imnodes_EndInputAttribute();

                    ImnodesNative.imnodes_BeginOutputAttribute(id++, ImNodesPinShape.Circle);
                    ImguiNative.igIndent(40);
                    ImguiNative.igText("output");
                    ImnodesNative.imnodes_EndOutputAttribute();

                    ImnodesNative.imnodes_EndNode();
                }

                ImnodesNative.imnodes_MiniMap(0.25f, ImNodesMiniMapLocation.BottomRight, IntPtr.Zero, IntPtr.Zero);
                ImnodesNative.imnodes_EndNodeEditor();

                ImguiNative.igEnd();
            }
        }
    }
}
