using KalosfideAPI.Roles;
using KalosfideAPI.DétailCommandes;
using KalosfideAPI.Livraisons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using KalosfideAPI.Data.Constantes;
using KalosfideAPI.Data.Keys;

namespace KalosfideAPI.Data
{
    // états successifs d'une Commande
    //
    // Client
    // 0. création de la première commande à la création du client
    //
    // 1. création: LivraisonUid et LivraisonRno fixés, pas de Date, Etat nouveau
    // 2. ajout Detail avec Demande fixée
    // 3. envoi bon: Date fixée
    // 2. et 3. peuvent se répéter
    //
    // Fournisseur:
    // 0. création de la première livraison à la création du Fournisseur
    //
    // 1. enregistrement:
    //      refusée: Etat refusé
    //  ou  en préparation: Etat accepté, LivraisonNo fixé
    //
    // Client 1: pour avoir une commande encours
    //
    // 2. livraison: pour chaque commande acceptée
    //      prête pour le bon: tous les Détails.AServir fixés
    //      bon de livraison envoyé: Livraison.Date fixée Etat préparé
    //      créer nouvelle livraison vide
    //
    // 2. facture: pour chaque livraison sans
    //      prête pour la facture: tous les Détails.Servis fixés
    //      facture envoyée: Livraison.Date fixée
    public class Commande: AKeyUidRnoNo
    {
        // key
        [Required]
        [MaxLength(LongueurMax.UId)]
        public override string Uid { get; set; }
        [Required]
        public override int Rno { get; set; }
        [Required]
        public override long No { get; set; }

        // données
        // la date est fixée quand le bon de commande est envoyé
        public DateTime? Date { get; set; }

        [MaxLength(LongueurMax.UId)]
        public string LivraisonUid { get; set; }
        public int LivraisonRno { get; set; }
        public long? LivraisonNo { get; set; }

        [StringLength(1)]
        public string Etat { get; set; }

        // utiles
        public decimal? Prix
        {
            get
            {
                List<DétailCommande> détails= new List<DétailCommande>(DétailCommandes);
                decimal? prix = -1;
                détails.ForEach(d =>
                {
                    if (d.Prix.HasValue)
                    {
                        prix += d.Prix.Value;
                    }
                });
                return prix;
            }
        }

        // navigation
        virtual public Client Client { get; set; }
        virtual public Livraison Livraison { get; set; }
        virtual public ICollection<DétailCommande> DétailCommandes { get; set; }

        // création
        public static void CréeTable(ModelBuilder builder)
        {
            var entité = builder.Entity<Commande>();

            entité.Property(c => c.Etat).HasDefaultValue(TypeEtatCommande.Nouveau);

            entité.HasKey(donnée => new
            {
                donnée.Uid,
                donnée.Rno,
                donnée.No
            });

            entité
                .HasOne(c => c.Client)
                .WithMany(cl => cl.Commandes)
                .HasForeignKey(c => new { c.Uid, c.Rno });

            entité.ToTable("Commandes");
        }
    }
}
