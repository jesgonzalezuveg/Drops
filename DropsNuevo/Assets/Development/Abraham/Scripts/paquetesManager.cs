using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.SceneManagement;

public class paquetesManager : MonoBehaviour {

    appManager manager;
    public Image imagen;
    public GameObject textoPaquetes;
    public GameObject listaPaquetes;
    public GameObject listaPaquetesNuevos;
    private bool bandera = true;

    private void Awake() {
        manager = GameObject.Find("AppManager").GetComponent<appManager>();
    }

    private void Start() {
        manager.setBanderas(true);
        StartCoroutine(webServicePaquetes.getPaquetes());
        StartCoroutine(webServiceCategoria.getCategorias());
        StartCoroutine(webServiceAcciones.getAcciones());
        StartCoroutine(webServiceEjercicio.getEjercicios());
    }

    private void Update() {
        if (manager.getImagen() != "" && bandera) {
            StartCoroutine(getUserImg());
            bandera = false;
        }

        if (listaPaquetesNuevos.transform.childCount <= 0) {
            GameObject.Find("ListaPaquetes").GetComponent<paquetesManager>().textoPaquetes.SetActive(true);
        } else {
            GameObject.Find("ListaPaquetes").GetComponent<paquetesManager>().textoPaquetes.SetActive(false);
        }
    }


    IEnumerator getUserImg() {
        if (manager.GetComponent<appManager>().getImagen() != "") {
            string path = manager.GetComponent<appManager>().getImagen().Split('/')[manager.GetComponent<appManager>().getImagen().Split('/').Length - 1];
            if (File.Exists(Application.persistentDataPath + path)) {
                byte[] byteArray = File.ReadAllBytes(Application.persistentDataPath + path);
                Texture2D texture = new Texture2D(8, 8);
                texture.LoadImage(byteArray);
                Rect rec = new Rect(0, 0, texture.width, texture.height);
                var sprite = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
                imagen.sprite = sprite;
            } else {
                WWW www = new WWW(manager.GetComponent<appManager>().getImagen());
                yield return www;
                Texture2D texture = www.texture;
                byte[] bytes = texture.EncodeToPNG();
                File.WriteAllBytes(Application.persistentDataPath + path, bytes);
                Rect rec = new Rect(0, 0, texture.width, texture.height);
                var sprite = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
                imagen.sprite = sprite;
            }
        }
    }

    public void newCardJugar(webServicePaquetes.paqueteData pack) {
        var fichaPaquete = Instantiate(Resources.Load("fichaPaqueteJugar") as GameObject);
        fichaPaquete.name = "fichaPack" + pack.id;
        StartCoroutine(llenarFicha(fichaPaquete, pack.urlImagen));
        fichaPaquete.transform.SetParent(listaPaquetes.transform);
        fichaPaquete.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        fichaPaquete.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
        fichaPaquete.GetComponent<packManager>().paquete = pack;
    }

    public void newCardActualizar(webServicePaquetes.paqueteData pack) {
        var fichaPaquete = Instantiate(Resources.Load("fichaPaqueteActualizar") as GameObject);
        fichaPaquete.name = "fichaPack" + pack.id;
        StartCoroutine(llenarFicha(fichaPaquete, pack.urlImagen));
        fichaPaquete.transform.SetParent(listaPaquetes.transform);
        fichaPaquete.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        fichaPaquete.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
        fichaPaquete.GetComponent<packManager>().paquete = pack;
    }

    public void newCardDescarga(webServicePaquetes.paqueteData pack) {
        var fichaPaquete = Instantiate(Resources.Load("fichaPaquete") as GameObject);
        fichaPaquete.name = "fichaPack" + pack.id;
        StartCoroutine(llenarFicha(fichaPaquete, pack.descripcion, pack.urlImagen));
        fichaPaquete.transform.SetParent(listaPaquetesNuevos.transform);
        fichaPaquete.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        fichaPaquete.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
        fichaPaquete.GetComponent<packManager>().paquete = pack;
    }

    public void fillEmpty() {
        var hijos = listaPaquetes.GetComponentsInChildren<packManager>(true);
        if (hijos.Length <= 5) {
            var obj = Instantiate(Resources.Load("placeHolder")) as GameObject;
            obj.transform.position = new Vector3(0, 0, 0);
            obj.transform.SetParent(listaPaquetes.transform);
            obj.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            obj.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
            fillEmpty();
        }else if (hijos.Length % 2 == 1) {
            Debug.Log("Insertar otro");
            var obj = Instantiate(Resources.Load("placeHolder")) as GameObject;
            obj.transform.position = new Vector3(0, 0, 0);
            obj.transform.SetParent(listaPaquetes.transform);
            obj.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            obj.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
        }
        listaPaquetes.GetComponent<gridScrollLayout>().bandera = true;
        listaPaquetesNuevos.GetComponent<gridScrollLayout>().bandera = true;
    }

    IEnumerator llenarFicha(GameObject ficha, string descripcion, string urlImagen) {
        ficha.transform.GetChild(1).GetComponent<Text>().text = descripcion;

        string path = urlImagen.Split('/')[urlImagen.Split('/').Length - 1];
        if (File.Exists(Application.persistentDataPath + path)) {
            byte[] byteArray = File.ReadAllBytes(Application.persistentDataPath + path);
            Texture2D texture = new Texture2D(8, 8);
            texture.LoadImage(byteArray);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            var sprite = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
            ficha.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
        } else {
            WWW www = new WWW(urlImagen);
            yield return www;
            Texture2D texture = www.texture;
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(Application.persistentDataPath + path, bytes);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            var sprite = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
            ficha.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
        }
    }

    IEnumerator llenarFicha(GameObject ficha, string urlImagen) {
        string path = urlImagen.Split('/')[urlImagen.Split('/').Length - 1];
        if (File.Exists(Application.persistentDataPath + path)) {
            byte[] byteArray = File.ReadAllBytes(Application.persistentDataPath + path);
            Texture2D texture = new Texture2D(8, 8);
            texture.LoadImage(byteArray);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            var sprite = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
            ficha.GetComponent<Image>().sprite = sprite;
        } else {
            WWW www = new WWW(urlImagen);
            yield return www;
            Texture2D texture = www.texture;
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(Application.persistentDataPath + path, bytes);
            Rect rec = new Rect(0, 0, texture.width, texture.height);
            var sprite = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
            ficha.GetComponent<Image>().sprite = sprite;
        }
    }

    public void salir() {
        Application.Quit();
    }
}
