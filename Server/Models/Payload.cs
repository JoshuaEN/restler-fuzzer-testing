using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public enum PayloadEnum {
        Apple,
        Orange,
        Lemon,
        Lime
    }
    public enum PayloadEnum2
    {
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
        M,
        N,
        O,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z
    }

    public enum PayloadEnum3
    {
        Spring,
        Summer,
        Fall,
        Winter
    }
    public enum PayloadEnum4
    {
        Sunny,
        Rain,
        Thunderstorms,
        Windy,
        Snow,
    }

    public class Payload : IInMemoryModel<string> {
        public PayloadEnum TestType { get; set; }
        public string TestString {get; set;}
        public string TestValueA { get; set; }
        public string TestValueB { get; set; }
        public string TestValueC { get; set; }

        public PayloadEnum2 Letter { get; set; }

        public PayloadEnum3 Season { get; set; }
        public PayloadEnum4 Weather { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string InternalIdentifier { get; set; }
    }
}
