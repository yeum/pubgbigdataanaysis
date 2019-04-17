using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class BattleGroundTest : MonoBehaviour {

    string url = "https://api.pubg.com/shards/pc-krjp/players?filter[playerNames]=XiGua520777";  //최대 14일전 데이터를 가지고있음. // 해당 플레이어 닉네임 기반 데이터 호출
    // string url = "https://api.pubg.com/shards/steam/seasons";  // 해당 시즌 데이터 호출
    //string url = "https://api.pubg.com/shards/pc-as/players/account.5aa8df8a76e54804aa2a7796c87e8912/seasons/division.bro.official.pc-2018-03";  // 해당 플레이어 ID 를 통해서 원하는 시즌 데이터 호출

    //string url = "https://api.pubg.com/shards/steam/players/account.5aa8df8a76e54804aa2a7796c87e8912/seasons/lifetime";  // 안씀
    //string url = "https://api.pubg.com/shards/$platform/matches/344ba2ad-413f-41a5-a098-50f11ab4185f";  // 해당 매치에 대한 데이터 호출 => 해당 매치 included 안에서 asset id를 통해서 telemetry 주소 값을 호출
    // string url = "https://api.pubg.com/shards/steam/leaderboards/solo";
    //  string url = "https://api.pubg.com/tournaments";

    // string url = "https://api.pubg.com/tournaments/kr-bj18s1";
    // string url = "https://api.pubg.com/shards/steam/matches/a81475bd-8440-4aa6-8a0c-53eb6efb40a3";
    //string url = "https://api.pubg.com/shards/steam/matches/4df23011-6113-11e9-acbc-0a586464d80a";
    // string url = "https://telemetry-cdn.playbattlegrounds.com/bluehole-pubg/pc-ru/2019/04/05/11/23/379a2e6b-5795-11e9-88ee-0a5864640007-telemetry.json";  // 해당 match url 호출로 통해서 telemetry url 호출 주소

    //String url = "https://api.pubg.com/shards/$platform/matches/3506dda9-1cb3-47ec-9f67-2acf0bfeab72";
    // Use this for initialization
    void Start () {
        StartCoroutine(GetRequest(url));
        //StartCoroutine(GetRequestbyMatchId(url));


    }



    IEnumerator GetRequest(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);

        //www.SetRequestHeader("Authorization", "Bearer    eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJqdGkiOiJkNGRmNWQxMC1kZTRiLTAxMzYtZmQ5Ny0wOTM1YTc1NjQ2NjAiLCJpc3MiOiJnYW1lbG9ja2VyIiwiaWF0IjoxNTQ0NDA2NjAxLCJwdWIiOiJibHVlaG9sZSIsInRpdGxlIjoicHViZyIsImFwcCI6Ii0xZGFiN2NiYS0yNzMwLTQ1YmItOTVjNi00NzI2NWFiM2VhMzMifQ.ZTW3uzO9LvOV3UhFNdXPdI-lhqqBO0xFmvhGvNSqwRQ");  // 해당 배틀 그라운드 API key 값 
        www.SetRequestHeader("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJqdGkiOiIxYWJlNmIyMC0zMWE1LTAxMzctNDFmMi0wMDU3NDUzNGQzNjMiLCJpc3MiOiJnYW1lbG9ja2VyIiwiaWF0IjoxNTUzNTcwODkwLCJwdWIiOiJibHVlaG9sZSIsInRpdGxlIjoicHViZyIsImFwcCI6Inl1bSJ9.vAryp8FbjpH3xl-HqU7LQ6dx8SR9TULy4zitl6ckHPg");  // 해당 배틀 그라운드 API key 값 
        www.SetRequestHeader("Accept", "application/vnd.api+json");  // Accept 방식 설정
      //  www.SetRequestHeader("Accept-Encoding", "gzip");


        yield return www.SendWebRequest();
       
        if (www.isNetworkError)
        {
            Debug.Log("Error While Sending: " + www.error);
        }
        else
        {
            Debug.Log("Received: " + www.downloadHandler.text);

            var Jsondata = JSON.Parse(www.downloadHandler.text);

            Debug.Log(Jsondata["data"][0]["relationships"]["matches"]["data"].Count);

            //for (int i = 0; i < Jsondata["data"][0]["relationships"]["matches"]["data"].Count; i++) {
            //    Debug.Log(Jsondata["data"][0]["relationships"]["matches"]["data"][i]["id"].ToString().Replace("\"",""));
            //}

            // 일단 매치의 id는 가져왔는데.. 얘를 어떻게 다시 주소에 넣어서 가져오지?

            string url1 = "https://api.pubg.com/shards/$platform/matches/" + Jsondata["data"][0]["relationships"]["matches"]["data"][0]["id"].ToString().Replace("\"", "");
            StartCoroutine(GetRequestbyMatchId(url1));
            /*
            Debug.Log(Jsondata["data"]["relationships"]["rosters"]["data"].Count);

            for (int i = 0; i < Jsondata["data"]["relationships"]["rosters"]["data"].Count; i++) {

                Debug.Log(Jsondata["data"]["relationships"]["rosters"]["data"][i]["id"]);
            }
            */
            /*
            Debug.Log(Jsondata["included"].Count);
            
            for (int i =0; i < Jsondata["included"].Count ; i++) {


                for (int j = 0; j < Jsondata["data"]["relationships"]["rosters"]["data"].Count; j++)
                {

                    if (Jsondata["included"][i]["id"].ToString().Replace("\"", "").Equals(Jsondata["data"]["relationships"]["rosters"]["data"][j]["id"].ToString().Replace("\"", "")))
                    {
                        Debug.Log("Contain Ok : " + Jsondata["included"][i]["type"].ToString().Replace("\"", ""));


                    }
                    else {

                        Debug.Log("Contain No : " + Jsondata["included"][i]["type"].ToString().Replace("\"", ""));

                    }      
                }

            }
            */

            /*
            string a = "";
            string b = "";

            for (int i = 0; i < Jsondata["included"].Count; i++)
            {
             //   a += i+ " " + Jsondata["included"][i]["relationships"]["participants"]["data"][0]["id"].ToString().Replace("\"", "") + "\n";
              //  b += i+ " " + Jsondata["included"][i]["id"].ToString().Replace("\"", "") + "\n";
                //    Debug.Log("id : " + Jsondata["included"][i]["relationships"]["participants"]["data"][0]["id"].ToString().Replace("\"", ""));
                //   Debug.Log("id : " + Jsondata["included"][i]["relationships"]["participants"]["data"]["id"].ToString().Replace("\"", ""));

                //  Debug.Log("count id : " +Jsondata["included"][i]["id"].ToString().Replace("\"", ""));

                    if (Jsondata["included"][i]["id"].ToString().Replace("\"", "").Equals("379a2e6b-5795-11e9-88ee-0a5864640007"))
                    {
                        Debug.Log("Contain Ok : " + Jsondata["included"][i].ToString().Replace("\"", ""));


                    }
                

            }
            
             //   Debug.Log(a);
                Debug.Log(b);

    */


            // byte[] bytesNew = System.Convert.FromBase64String(www.downloadHandler.text);

            // Debug.Log(Decompress(bytesNew));
        }
    }

    IEnumerator GetRequestbyMatchId(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);

        //www.SetRequestHeader("Authorization", "Bearer    eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJqdGkiOiJkNGRmNWQxMC1kZTRiLTAxMzYtZmQ5Ny0wOTM1YTc1NjQ2NjAiLCJpc3MiOiJnYW1lbG9ja2VyIiwiaWF0IjoxNTQ0NDA2NjAxLCJwdWIiOiJibHVlaG9sZSIsInRpdGxlIjoicHViZyIsImFwcCI6Ii0xZGFiN2NiYS0yNzMwLTQ1YmItOTVjNi00NzI2NWFiM2VhMzMifQ.ZTW3uzO9LvOV3UhFNdXPdI-lhqqBO0xFmvhGvNSqwRQ");  // 해당 배틀 그라운드 API key 값 
        www.SetRequestHeader("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJqdGkiOiIxYWJlNmIyMC0zMWE1LTAxMzctNDFmMi0wMDU3NDUzNGQzNjMiLCJpc3MiOiJnYW1lbG9ja2VyIiwiaWF0IjoxNTUzNTcwODkwLCJwdWIiOiJibHVlaG9sZSIsInRpdGxlIjoicHViZyIsImFwcCI6Inl1bSJ9.vAryp8FbjpH3xl-HqU7LQ6dx8SR9TULy4zitl6ckHPg");  // 해당 배틀 그라운드 API key 값 
        www.SetRequestHeader("Accept", "application/vnd.api+json");  // Accept 방식 설정
                                                                     //  www.SetRequestHeader("Accept-Encoding", "gzip");


        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("Error While Sending: " + www.error);
        }
        else
        {
            Debug.Log("ReceivedMatch: " + www.downloadHandler.text);

            var Jsondata = JSON.Parse(www.downloadHandler.text);

            string url2 = "";
            
            //Debug.Log(Jsondata["included"].Count);

            for (int i = 0; i < Jsondata["included"].Count; ++i)
            {
                if(Jsondata["included"][i]["type"].ToString().Replace("\"", "").Equals("asset"))
                {
                    // included 배열 내에서 type이 asset인 것은 딱 하나. 얘를 찾아서 attributes내의 URL을 telemetry를 불러내는데 쓸 수 있도록 저장한다. 
                    url2 = Jsondata["included"][i]["attributes"]["URL"].ToString().Replace("\"", "");
                }
                if (Jsondata["included"][i]["type"].ToString().Replace("\"", "").Equals("participant"))
                {
                    //Debug.Log(Jsondata["included"][i]["id"].ToString().Replace("\"", ""));
                }
            }
            StartCoroutine(GetRequestbyAssetId(url2));

            // 자세한 매치 정보는 어떻게 찾는가
            // account.이 달린 ID와 그냥 ID는 무슨 차이?
            // 구조파악해야할듯

        }
    }

    IEnumerator GetRequestbyAssetId(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);

        //www.SetRequestHeader("Authorization", "Bearer    eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJqdGkiOiJkNGRmNWQxMC1kZTRiLTAxMzYtZmQ5Ny0wOTM1YTc1NjQ2NjAiLCJpc3MiOiJnYW1lbG9ja2VyIiwiaWF0IjoxNTQ0NDA2NjAxLCJwdWIiOiJibHVlaG9sZSIsInRpdGxlIjoicHViZyIsImFwcCI6Ii0xZGFiN2NiYS0yNzMwLTQ1YmItOTVjNi00NzI2NWFiM2VhMzMifQ.ZTW3uzO9LvOV3UhFNdXPdI-lhqqBO0xFmvhGvNSqwRQ");  // 해당 배틀 그라운드 API key 값 
        www.SetRequestHeader("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJqdGkiOiIxYWJlNmIyMC0zMWE1LTAxMzctNDFmMi0wMDU3NDUzNGQzNjMiLCJpc3MiOiJnYW1lbG9ja2VyIiwiaWF0IjoxNTUzNTcwODkwLCJwdWIiOiJibHVlaG9sZSIsInRpdGxlIjoicHViZyIsImFwcCI6Inl1bSJ9.vAryp8FbjpH3xl-HqU7LQ6dx8SR9TULy4zitl6ckHPg");  // 해당 배틀 그라운드 API key 값 
        www.SetRequestHeader("Accept", "application/vnd.api+json");  // Accept 방식 설정
                                                                     //  www.SetRequestHeader("Accept-Encoding", "gzip");


        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("Error While Sending: " + www.error);
        }
        else
        {
            Debug.Log("ReceivedMatch: " + www.downloadHandler.text);

            var Jsondata = JSON.Parse(www.downloadHandler.text);

            int count = 0;
            for(int i=0;i<Jsondata.Count;++i)
            {
                if(Jsondata[i]["_T"].ToString().Replace("\"","").Equals("LogParachuteLanding"))
                {
                    count++;
                }
            }
            Debug.Log(count);

            // 19.04.17
            // 자세한 매치 정보는 어떻게 찾는가
            // account.이 달린 ID와 그냥 ID는 무슨 차이?
            // 구조파악해야할듯

            // 19.04.18
            // 매치 정보 찾음
            // 그러나 JSON Parse: Too many closing brackets 오류 뜸 파일이 너무 커서 그런듯
            // 큰 파일을 어떻게 잘라서 받지?


        }
    }

    static byte[] Decompress(byte[] gzip)
    {
        // Create a GZIP stream with decompression mode.
        // ... Then create a buffer and write into while reading from the GZIP stream.
        using (GZipStream stream = new GZipStream(new MemoryStream(gzip),
            CompressionMode.Decompress))
        {
            const int size = 4096;
            byte[] buffer = new byte[size];
            using (MemoryStream memory = new MemoryStream())
            {
                int count = 0;
                do
                {
                    count = stream.Read(buffer, 0, size);
                    if (count > 0)
                    {
                        memory.Write(buffer, 0, count);
                    }
                }
                while (count > 0);
                return memory.ToArray();
            }
        }
    }
}
