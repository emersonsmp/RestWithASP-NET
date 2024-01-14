﻿using RestWithASPNET.Hypermedia;
using RestWithASPNET.Hypermedia.Abstract;
using RestWithASPNET.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestWithASPNET.Data.VO
{
    public class PersonVO: ISupportsHyperMedia
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public List<HyperMediaLink> Links { get; set; } =  new List<HyperMediaLink>();
    }
}
