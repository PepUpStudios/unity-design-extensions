using UnityEngine.UI;

public class Health {

    public float value {
        get {
            return m_health;
        } set {
            m_health = value;
            if (OnEnd != null && m_health <= min) {
                OnEnd();
            }
            if (OnReset != null && m_health >= max) {
                OnReset();
            }
        }
    }

    public Slider[] barUIs = null;
    public Text[] textUIs = null;

    public float min;
    public float max;

    private float m_health;

    public delegate void HealthUpdate();

    public event HealthUpdate OnUpdate;
    public event HealthUpdate OnEnd;
    public event HealthUpdate OnReset;

    public Health(float initialHealth, float min = 0, float max = 100) {
        value = initialHealth;
        this.min = min;
        this.max = max;
    }

    public Health() {
        value = 100f;
        min = 0f;
        max = 100f;
    }

    public void ReduceBy(float amount, bool updateUI = true) {
        m_health -= amount;
        if (OnUpdate != null) {
            OnUpdate();
        }
        if (m_health <= min) {
            m_health = min;
            if (OnEnd != null) {
                OnEnd();
            }
        }
    }

    public void AddBy(float amount, bool updateUI = true) {
        bool reset = false;
        m_health += amount;
        if (m_health > max) {
            m_health = max;
            reset = true;
        }
        if (updateUI) {
            UpdateAllUI();
        }
        if (OnUpdate != null) {
            OnUpdate();
        }
        if (OnReset != null && reset) {
            OnReset();
        }
    }

    public void UpdateAllUI() {
        UpdateBarUI();
        UpdateTextUI();
    }

    public void UpdateBarUI() {
        if (barUIs != null) {
            foreach (Slider bar in barUIs) {
                if (bar != null) {
                    bar.value = m_health;
                }
            }
        }
    }

    public void UpdateTextUI() {
        if (textUIs != null) {
            foreach (Text text in textUIs) {
                if (text) {
                    text.text = m_health.ToString();
                }
            }
        }
    }
}
