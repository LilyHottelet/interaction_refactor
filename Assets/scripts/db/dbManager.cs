using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using System;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class dbManager : MonoBehaviour {

	public string host, db, username, password;
	MySqlConnection con;

	public InputField email;
	public Text infoConnect;
	public Animator fadeAnimator;
	public Image black;


	void Start()
	{

		ConnectToDB ();
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
	void ConnectToDB()
	{
		string cmd = "SERVER = " + host + ";" + "database = " + db + ";User ID = " + username + ";Password =" + password + ";Pooling = true;Charset = utf8";

		try
		{
			con = new MySqlConnection(cmd);
			con.Open();
		} catch (Exception ex)
		{
			print (ex.ToString());
		}
	}

	void Update()
	{
		Debug.Log (con.State);
	}

	bool emailisValid()
	{
		
		Regex regex = new Regex (@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
		Match match = regex.Match(email.text);
		if (match.Success)
		{
			return true;
		} else
		{
			return false;
		}
	}

	public void RemovePlaceHolder()
	{
		email.placeholder.enabled = false;
	}

	public void SignUp()
	{
		
		if (emailisValid())
		{ 
			string cmd ="INSERT INTO `userInfo` (`email`) VALUES ('"+email.text+"')";
			MySqlCommand request = new MySqlCommand (cmd, con);
			try
			{
				request.ExecuteReader();
				print("success");
				infoConnect.text = "Thank you for your time.";
				StartCoroutine(GoBackToMenu());
			} catch (Exception ex)
			{
				print (ex.ToString ());
			}
		} else
		{
			infoConnect.text = "Please enter a valid email.";
			StartCoroutine (EraseText ());
		}

	}

	public void NoThanks()
	{
		StartCoroutine (GoBackToMenu ());
	}

	IEnumerator EraseText()
	{
		yield return new WaitForSeconds(3f);
		infoConnect.text = "";
	}

	IEnumerator GoBackToMenu()
	{
		yield return new WaitForSeconds (3f);
		fadeAnimator.SetBool ("fade", true);
		yield return new WaitUntil (() => black.color.a == 1);
		SceneManager.LoadScene ("menu");

	}


}
