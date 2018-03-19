using UnityEngine;
using TMPro;

public class TextMeshCounter : NonLiving {
    
    public string prefixString = "";
    public string suffixString = "";
    [Tooltip("Animator bool trigger value.")]
    public string animTrigger = "";
    public string poolTag = "";

    protected TextMeshProUGUI textMesh = null;

    public void Activate(string text) {
        if (textMesh == null) textMesh = this.GetComponent<TextMeshProUGUI>();
        textMesh.text = prefixString + text + suffixString;
    }

    public void Deactive() {
        PutObjectToPool();
    }

    protected void PutObjectToPool() {
        pool.PutObject(poolTag, this.name, this);
        this.gameObject.SetActive(false);
    }
}