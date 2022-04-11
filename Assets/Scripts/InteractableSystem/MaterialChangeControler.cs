using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChangeControler : MonoBehaviour
{
    [Serializable]
    public class MaterialTarget {
        public Material activatedMaterial, deactivatedMaterial;
        public int materialIndex = 0;
    }

    [SerializeField] private MeshRenderer _mesh = null;
    [SerializeField] private MaterialTarget[] _materialTargets;

    public void ChangeMaterialToActivated() {
        Material[] materialArray = _mesh.materials;
        Debug.Log(materialArray);
        
        foreach (MaterialTarget materialTarget in _materialTargets) {
            materialArray[materialTarget.materialIndex] = materialTarget.activatedMaterial;
        }

        _mesh.materials = materialArray;
    }
    
    [ContextMenu("DEACTIVATE")]
    public void ChangeMaterialToDeactivated() {
        Material[] materialArray = _mesh.materials;
        
        foreach (MaterialTarget materialTarget in _materialTargets) {
            materialArray[materialTarget.materialIndex] = materialTarget.deactivatedMaterial;
        }
        
        _mesh.materials = materialArray;
    }
}
