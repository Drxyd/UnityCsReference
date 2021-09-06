// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Rendering
{
    [UsedByNativeCode]
    [NativeHeader("Runtime/Shaders/RayTracingAccelerationStructure.h")]
    [NativeHeader("Runtime/Export/Graphics/RayTracingAccelerationStructure.bindings.h")]

    [Flags]
    public enum RayTracingSubMeshFlags
    {
        Disabled            = 0,
        Enabled             = (1 << 0),
        ClosestHitOnly      = (1 << 1),
        UniqueAnyHitCalls   = (1 << 2),
    }

    public sealed class RayTracingAccelerationStructure : IDisposable
    {
        [Flags]
        public enum RayTracingModeMask
        {
            Nothing             = 0,
            Static              = (1 << RayTracingMode.Static),
            DynamicTransform    = (1 << RayTracingMode.DynamicTransform),
            DynamicGeometry     = (1 << RayTracingMode.DynamicGeometry),
            Everything          = (Static | DynamicTransform | DynamicGeometry)
        }

        public enum ManagementMode
        {
            Manual      = 0,    // Manual management of Renderers in the Raytracing Acceleration Structure.
            Automatic   = 1,    // New renderers are added automatically based on a RayTracingModeMask.
        }

        public struct RASSettings
        {
            public ManagementMode managementMode;
            public RayTracingModeMask rayTracingModeMask;
            public int layerMask;
            public RASSettings(ManagementMode sceneManagementMode, RayTracingModeMask rayTracingModeMask, int layerMask)
            {
                this.managementMode = sceneManagementMode;
                this.rayTracingModeMask = rayTracingModeMask;
                this.layerMask = layerMask;
            }
        }

        // --------------------------------------------------------------------
        // IDisposable implementation, with Release() for explicit cleanup.

        ~RayTracingAccelerationStructure()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Release native resources
                Destroy(this);
            }

            m_Ptr = IntPtr.Zero;
        }

        // --------------------------------------------------------------------
        // Actual API
        public RayTracingAccelerationStructure(RASSettings settings)
        {
            m_Ptr = Create(settings);
        }

        public RayTracingAccelerationStructure()
        {
            RASSettings settings = new RASSettings();
            settings.rayTracingModeMask = RayTracingModeMask.Everything;
            settings.managementMode = ManagementMode.Manual;
            settings.layerMask = -1;
            m_Ptr = Create(settings);
        }

        [FreeFunction("RayTracingAccelerationStructure_Bindings::Create")]
        extern private static IntPtr Create(RASSettings desc);

        [FreeFunction("RayTracingAccelerationStructure_Bindings::Destroy")]
        extern private static void Destroy(RayTracingAccelerationStructure accelStruct);

        public void Release()
        {
            Dispose();
        }

#pragma warning disable 414
        internal IntPtr m_Ptr;
#pragma warning restore 414

        public void Build()
        {
            Build(Vector3.zero);
        }

        [Obsolete("Method Update has been deprecated. Use Build instead (UnityUpgradable) -> Build()", true)]
        public void Update()
        {
            Build(Vector3.zero);
        }

        [FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::Build", HasExplicitThis = true)]
        extern public void Build(Vector3 relativeOrigin);

        [Obsolete("Method Update has been deprecated. Use Build instead (UnityUpgradable) -> Build(*)", true)]
        [FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::Update", HasExplicitThis = true)]
        extern public void Update(Vector3 relativeOrigin);

        [FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::AddInstanceDeprecated", HasExplicitThis = true)]
        extern public void AddInstance([NotNull] Renderer targetRenderer, bool[] subMeshMask = null, bool[] subMeshTransparencyFlags = null, bool enableTriangleCulling = true, bool frontTriangleCounterClockwise = false, uint mask = 0xFF, uint id = 0xFFFFFFFF);

        public void AddInstance(Renderer targetRenderer, RayTracingSubMeshFlags[] subMeshFlags, bool enableTriangleCulling = true, bool frontTriangleCounterClockwise = false, uint mask = 0xFF, uint id = 0xFFFFFFFF)
        {
            AddInstanceSubMeshFlagsArray(targetRenderer, subMeshFlags, enableTriangleCulling, frontTriangleCounterClockwise, mask, id);
        }

        [FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::RemoveInstance", HasExplicitThis = true)]
        extern public void RemoveInstance([NotNull] Renderer targetRenderer);

        public void AddInstance(GraphicsBuffer aabbBuffer, uint numElements, Material material, bool isCutOff, bool enableTriangleCulling = true, bool frontTriangleCounterClockwise = false, uint mask = 0xFF, bool reuseBounds = false, uint id = 0xFFFFFFFF)
        {
            AddInstance_Procedural(aabbBuffer, numElements, material, Matrix4x4.identity, isCutOff, enableTriangleCulling, frontTriangleCounterClockwise, mask, reuseBounds, id);
        }

        public void AddInstance(GraphicsBuffer aabbBuffer, uint numElements, Material material, Matrix4x4 instanceTransform, bool isCutOff, bool enableTriangleCulling = true, bool frontTriangleCounterClockwise = false, uint mask = 0xFF, bool reuseBounds = false, uint id = 0xFFFFFFFF)
        {
            AddInstance_Procedural(aabbBuffer, numElements, material, instanceTransform, isCutOff, enableTriangleCulling, frontTriangleCounterClockwise, mask, reuseBounds, id);
        }

        [FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::AddInstance", HasExplicitThis = true)]
        extern private void AddInstance_Procedural([NotNull] GraphicsBuffer aabbBuffer, uint numElements, [NotNull] Material material, Matrix4x4 instanceTransform, bool isCutOff, bool enableTriangleCulling = true, bool frontTriangleCounterClockwise = false, uint mask = 0xFF, bool reuseBounds = false, uint id = 0xFFFFFFFF);

        [FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::UpdateInstanceTransform", HasExplicitThis = true)]
        extern public void UpdateInstanceTransform([NotNull] Renderer renderer);

        [FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::UpdateInstanceMask", HasExplicitThis = true)]
        extern public void UpdateInstanceMask([NotNull] Renderer renderer, uint mask);

        [FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::UpdateInstanceID", HasExplicitThis = true)]
        extern public void UpdateInstanceID([NotNull] Renderer renderer, uint instanceID);

        [FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::GetSize", HasExplicitThis = true)]
        extern public UInt64 GetSize();

        [FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::GetInstanceCount", HasExplicitThis = true)]
        extern public UInt32 GetInstanceCount();

        [FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::ClearInstances", HasExplicitThis = true)]
        extern public void ClearInstances();

        [FreeFunction(Name = "RayTracingAccelerationStructure_Bindings::AddInstanceSubMeshFlagsArray", HasExplicitThis = true)]
        extern private void AddInstanceSubMeshFlagsArray([NotNull] Renderer targetRenderer, RayTracingSubMeshFlags[] subMeshFlags, bool enableTriangleCulling = true, bool frontTriangleCounterClockwise = false, uint mask = 0xFF, uint id = 0xFFFFFFFF);
    }
}
