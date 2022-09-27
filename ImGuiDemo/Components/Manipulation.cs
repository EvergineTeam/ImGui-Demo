using Evergine.Bindings.Imgui;
using Evergine.Bindings.Imguizmo;
using Evergine.Framework;
using Evergine.Framework.Graphics;
using Evergine.Mathematics;
using Evergine.UI;
using System;
using System.Runtime.CompilerServices;

namespace ImGuiDemo.Components
{
    public unsafe class Manipulation : Behavior
    {
        [BindComponent]
        private Transform3D transform = null;

        private Matrix4x4 view;
        private Matrix4x4 projection;
        private Matrix4x4 world;

        private float[] bounds;
        private OPERATION operation;

        public Manipulation()
        {
            this.bounds = new[] { -0.5f, -0.5f, -0.5f, 0.5f, 0.5f, 0.5f };
            this.operation = OPERATION.TRANSLATE | OPERATION.BOUNDS;
        }

        protected override void Update(TimeSpan gameTime)
        {
            var io = ImguiNative.igGetIO();
            ImguizmoNative.ImGuizmo_SetRect(0, 0, io->DisplaySize.X, io->DisplaySize.Y);

            var camera = this.Managers.RenderManager.ActiveCamera3D;
            this.view = camera.View;
            this.projection = camera.Projection;
            this.world = this.transform.WorldTransform;

            float* boundsPointer = (float*)Unsafe.AsPointer(ref bounds[0]);
            ImguizmoNative.ImGuizmo_Manipulate(view.Pointer(), projection.Pointer(), this.operation, MODE.WORLD, world.Pointer(), null, null, boundsPointer, null);

            this.transform.WorldTransform = this.world;
        }
    }
}
