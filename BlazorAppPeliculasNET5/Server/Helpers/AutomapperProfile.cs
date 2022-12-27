using AutoMapper;
using BlazorAppPeliculasNET5.Shared.Entidades;

namespace BlazorAppPeliculasNET5.Server.Helpers
{
    public class AutomapperProfile:Profile
    {
        public AutomapperProfile() {
            CreateMap<Actor, Actor>()
                .ForMember(x => x.Foto, option => option.Ignore());

            CreateMap<Pelicula,Pelicula>()
                .ForMember(x => x.Poster, option => option.Ignore());
        }

    }
}
