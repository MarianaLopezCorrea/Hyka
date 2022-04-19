using Hyka.Models;
using Microsoft.EntityFrameworkCore;

namespace Hyka.Data
{
    //0372300114����������������������������PubDSK_1����������������531445����1003510411LOPEZ������������������������������������CORREA����������������������������������MARIANA������������������������������������������������������������������������������0F20020813150760O+��23��bMÿ€€uc}Xž¤†…t{_u„zŒWq’dœRŠ:‘G”Q‘ŒgŸa¤]A€Q{¬_ŸdÀcºx¾€¾Ž¼žµ˜¶€¶Œ|p§a(Œ?–LžRŽDˆ¸aH~ªj5oÀoRbWi“rhV¯†ŽzuS˜€‡~(ÑTÝOeÿÞ0um>«þ����������������������������������������������������������������������7/��TGÿ€€€¡h”eŸY–—}G”°iK‡ºg*u'gKpBjbSjR}¨…\‰mŽƒ„Œ‹¡_¡u££‹¥‡¦|§a´n«t·j³€¨·y,"m?y3jW^`b_mpb¢{W†SmpÚ|«ïé¹0OIUî«SÒ����������������������������������������������������������������������������������������XQŒ¤2õÌåñ!@«NtF7žÊËï[wÏ>g‹6ÊÍ˜ÊOw«•±|Æû
    //0372300114����������������������������PubDSK_1����������������531445����1003510411LOPEZ������������������������������������CORREA����������������������������������MARIANA������������������������������������������������������������������������������0F20020813150760O+��23��bMÿ€€uc}Xž¤†…t{_u„zŒWq’dœRŠ:‘G”Q‘ŒgŸa¤]A€Q{¬_ŸdÀcºx¾€¾Ž¼žµ˜¶€¶Œ|p§a(Œ?–LžRŽDˆ¸aH~ªj5oÀoRbWi“rhV¯†ŽzuS˜€‡~(ÑTÝOeÿÞ0um>«þ����������������������������������������������������������������������7/��TGÿ€€€¡h”eŸY–—}G”°iK‡ºg*u'gKpBjbSjR}¨…\‰mŽƒ„Œ‹¡_¡u££‹¥‡¦|§a´n«t·j³€¨·y,"m?y3jW^`b_mpb¢{W†SmpÚ|«ïé¹0OIUî«SÒ����������������������������������������������������������������������������������������XQŒ¤2õÌåñ!@«NtF7žÊËï[wÏ>g‹6ÊÍ˜ÊOw«•±|Æû
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Blockbuster> Blockbusters { get; set; }
        public DbSet<Person> Users { get; set; }
        public DbSet<Territory> Territories { get; set; }
        public DbSet<Tariff> Tariff { get; set; }


    }
}
