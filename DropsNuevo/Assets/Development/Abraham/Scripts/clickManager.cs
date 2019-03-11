using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class clickManager : MonoBehaviour {

    public AudioClip click;
    public AudioClip hover;
    private AudioSource source;
    private EventTrigger trigger;

    // Start is called before the first frame update
    void Start() {
        if (!this.GetComponent<AudioSource>()) {
            source = gameObject.AddComponent<AudioSource>();
        } else {
            source = gameObject.GetComponent<AudioSource>();
        }
        if (!this.GetComponent<EventTrigger>()) {
            trigger = gameObject.AddComponent<EventTrigger>();
        } else {
            trigger = gameObject.GetComponent<EventTrigger>();
        }
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => {
            source.clip = click;
            source.Play();
        });
        trigger.triggers.Add(entry);
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerEnter;
        entry2.callback.AddListener((data) => {
            source.clip = hover;
            source.Play();
        });
        trigger.triggers.Add(entry2);
        source.playOnAwake = false;
        source.clip = hover;
    }

    private void clickFunc() {
        source.clip = hover;
        source.Play();
    }

    private void hoverFunc() {
        source.clip = click;
        source.Play();
    }


}
