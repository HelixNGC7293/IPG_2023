using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class JsonReceiver2
{
	public Comment[] comments;
}

[System.Serializable]
public class Comment
{
	public int postId;
	public int id;
	public string name;
	public string email;
	public string body;
}