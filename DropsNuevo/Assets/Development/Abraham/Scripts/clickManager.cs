using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class clickManager : MonoBehaviour {

    public bool cambiarDialogoMascota;
    public string mensaje;

    private AudioClip click;        ///< click audioClip que almacena el audio de click 
    private AudioClip hover;        ///< hover audioClip que almacena el audio de hover
    private AudioSource source;     ///< source audioSource que reproducira los audioClips
    private EventTrigger trigger;   ///< trigger EventTrigger que manejara los eventos de hover y click

    /**
     * Funcion que se manda llamar al inicio de la aplicacion(frame 1)
     * Añade los componentes de tipo eventTrigger con sus respectivos eventos
     * obtiene de la carpeta resources los clips de audio de click y hover
     */
    void Start() {
        if (!this.GetComponent<OVRRaycaster>()) {
            gameObject.AddComponent<OVRRaycaster>();
        } else {
            gameObject.GetComponent<OVRRaycaster>();
        }

        click = Resources.Load("Sounds/click") as AudioClip;
        hover = Resources.Load("Sounds/hover") as AudioClip;

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
            if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad)) {
                return;
            }
            source.clip = click;
            source.Play();
        });
        trigger.triggers.Add(entry);
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerEnter;
        entry2.callback.AddListener((data) => {
            if (cambiarDialogoMascota) {
                GameObject.Find("Mascota").GetComponentInChildren<Text>().text = mensaje;
            }
            source.clip = hover;
            source.Play();
        });
        trigger.triggers.Add(entry2);
        source.playOnAwake = false;
        source.clip = hover;
    }
}
