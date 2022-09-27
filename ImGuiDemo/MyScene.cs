using Evergine.Framework;
using Evergine.Framework.Graphics;
using Evergine.UI;

namespace ImGuiDemo
{
    public class MyScene : Scene
    {
        public override void RegisterManagers()
        {
            base.RegisterManagers();
            this.Managers.AddManager(new global::Evergine.Bullet.BulletPhysicManager3D());
            this.Managers.AddManager(new ImGuiManager()
            {
                ImGuizmoEnabled = true,
                ImPlotEnabled = true,
                ImNodesEnabled = true,
            });
        }

        protected override void CreateScene()
        {
        }
    }
}