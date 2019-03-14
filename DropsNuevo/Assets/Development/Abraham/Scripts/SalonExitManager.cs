﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SalonExitManager : MonoBehaviour {

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
            source.clip = click;
            source.Play();
            StartCoroutine(wait());
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

    IEnumerator wait() {
        yield return new WaitUntil(() => gameObject.GetComponentInChildren<AudioSource>().isPlaying == false);
        SceneManager.LoadScene("menuCategorias");
    }
}