using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using T1808A_Asigntment.Entity;

namespace T1808A_Asigntment.Service
{
    class SongService : ISongService
    {
        public Song CreateSong(MemberCredential memberCredential, Song song)
        {
            // tạo đối tượng httpclient giúp gửi dữ liệu đi. (hoặc lấy dữ liệu về)
            var httpClient = new HttpClient();
            // chuyển kiểu dữ liệu c# thành kiểu dữ liệu json.
            var dataToSend = JsonConvert.SerializeObject(song);
            // gói gém, gắn mác cho dữ liệu gửi đi, xác định kiểu dữ liệu là json, encode là utf8.
            var content = new StringContent(dataToSend, Encoding.UTF8, "application/json");
            // gán token
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(memberCredential.token);
            // thực hiện gửi dữ liệu với phương thức post.
            var response = httpClient.PostAsync(ProjectConfiguration.SONG_CREATE_URL, content).GetAwaiter().GetResult();
            // lấy kết quả trả về từ server.
            var jsonContent = response.Content.ReadAsStringAsync().Result;
            // ép kiểu kết quả từ dữ liệu json sang dữ liệu của C#
            var responseSong = JsonConvert.DeserializeObject<Song>(jsonContent);
            // in ra id của member trả về.
            Debug.WriteLine("Create success with id: " + responseSong.id);
            return responseSong;
        }

        public List<Song> GetAllSong(MemberCredential memberCredential)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(memberCredential.token);
            var response = httpClient.GetAsync(ProjectConfiguration.SONG_GETALL_URL).GetAwaiter().GetResult();
            var listSong = JsonConvert.DeserializeObject<List<Song>>(response.Content.ReadAsStringAsync().Result);
            return listSong;
        }

        //public List<Credential> GetListSong(MemberLogin memberLogin)
        //{
        //    var httpClient = new HttpClient();
        //    var dataToSend = JsonConvert.SerializeObject(memberLogin);
        //    var content = new StringContent(dataToSend, Encoding.UTF8, "application/json");
        //    var response = httpClient.PostAsync(ProjectConfiguration.MEMBER_LOGIN_URL, content).GetAwaiter().GetResult();
        //    var memberCredential = JsonConvert.DeserializeObject<Credential>(response.Content.ReadAsStringAsync().Result);
        //    // Debug.WriteLine(memberCredential.token);
        //    // SaveTokenToFile(memberCredential);
        //    return memberCredential;
        //}

        public List<Song> GetMineSongs(MemberCredential memberCredential)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(memberCredential.token);
            var response = httpClient.GetAsync(ProjectConfiguration.SONG_GETMINE_URL).GetAwaiter().GetResult();
            var listSong = JsonConvert.DeserializeObject<List<Song>>(response.Content.ReadAsStringAsync().Result);
            return listSong;
        }
    }
}
