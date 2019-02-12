using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FBholder : MonoBehaviour {

    public Text friendsTxt;

    void Awake() {
        if (!FB.IsInitialized) {
            FB.Init(() => {
                if (FB.IsInitialized) {
                    FB.ActivateApp();
                } else {
                    Debug.Log("Fallo al iniciar");
                }
            }, isGameShown => {
                if (!isGameShown) {
                    Time.timeScale = 0;
                } else {
                    Time.timeScale = 1;
                }
            });
        } else {
            FB.ActivateApp();
        }
    }

    public void facebookLogin() {
        var permisos = new List<string>() { "public_profile","email","user_friends"};
        FB.LogInWithReadPermissions(permisos);
    }

    public void facebookLogout() {
        FB.LogOut();
    }

    public void facebookShare() {
        FB.ShareLink(new System.Uri("https://resocoder.com"), "Check it out", "Hola",new System.Uri("http://resocoder.com/wp-content/uploads/2017/01/logoRound512.png"));

    }


    public void facebookGameRequest() {
        FB.AppRequest("Come and play this awesome game", title: "Drops project");
    }

    public void facebookInvite() {
        //FB.Mobile.AppInvite(new System.Uri("https://play.google.com/store/apps/....."));
    }

    public void getFriendsPlayingThisGame() {
        string query = "/me/friends";
        FB.API(query, HttpMethod.GET, result => {
            var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
            var friendsList = (List<object>)dictionary["data"];
            friendsTxt.text = "";
            foreach (var friend in friendsList) {
                friendsTxt.text += ((Dictionary<string, object>)friend)["name"];
            }
        });
    }

}
