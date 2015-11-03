using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using System.ComponentModel;
using System.Configuration;


namespace Gymverksamhet_G3
{
    class Databasfunktioner
    {
        //VARIABLER        
        private const string conString = "grp3vt14";
        
        //METODER
        
        //Lägg till medlem med parametrar
        public static void AddMedlem(string regMedlemsnummer, string regFornamn, string regEfternamn, string regGatuadress, string regPostnummer, string regPostadress, string regTelefon, string regMailadress,
                         int regMedlemstypId, DateTime regStartdatum, DateTime regSlutdatum)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            NpgsqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                NpgsqlCommand command1 = new NpgsqlCommand(@"INSERT INTO medlem (medlemsnummer, fornamn, efternamn, gatuadress, postnummer, postadress, telefonnummer, mailadress, medlemstyp, startdatum, slutdatum)
                                                            VALUES (:newMedlemsnummer, :newFornamn, :newEfternamn, :newGatuadress, :newPostnummer, :newPostadress, :newTelefonnummer, :newMailadress, :newMedlemstypid, :newStartdatum, :newSlutdatum)", conn);

                command1.Parameters.Add(new NpgsqlParameter("newMedlemsnummer", NpgsqlDbType.Varchar));
                command1.Parameters["newMedlemsnummer"].Value = regMedlemsnummer;
                command1.Parameters.Add(new NpgsqlParameter("newFornamn", NpgsqlDbType.Varchar));
                command1.Parameters["newFornamn"].Value = regFornamn;
                command1.Parameters.Add(new NpgsqlParameter("newEfternamn", NpgsqlDbType.Varchar));
                command1.Parameters["newEfternamn"].Value = regEfternamn;
                command1.Parameters.Add(new NpgsqlParameter("newGatuadress", NpgsqlDbType.Varchar));
                command1.Parameters["newGatuadress"].Value = regGatuadress;
                command1.Parameters.Add(new NpgsqlParameter("newPostnummer", NpgsqlDbType.Varchar));
                command1.Parameters["newPostnummer"].Value = regPostnummer;
                command1.Parameters.Add(new NpgsqlParameter("newPostadress", NpgsqlDbType.Varchar));
                command1.Parameters["newPostadress"].Value = regPostadress;
                command1.Parameters.Add(new NpgsqlParameter("newTelefonnummer", NpgsqlDbType.Varchar));
                command1.Parameters["newTelefonnummer"].Value = regTelefon;
                command1.Parameters.Add(new NpgsqlParameter("newMailadress", NpgsqlDbType.Varchar));
                command1.Parameters["newMailadress"].Value = regMailadress;
                command1.Parameters.Add(new NpgsqlParameter("NewMedlemstypid", NpgsqlDbType.Integer));
                command1.Parameters["newMedlemstypid"].Value = regMedlemstypId;
                command1.Parameters.Add(new NpgsqlParameter("newStartdatum", NpgsqlDbType.Date));
                command1.Parameters["newStartdatum"].Value = regStartdatum;
                command1.Parameters.Add(new NpgsqlParameter("newSlutdatum", NpgsqlDbType.Date));
                command1.Parameters["newSlutdatum"].Value = regSlutdatum;

                command1.Transaction = trans;

                command1.ExecuteNonQuery();

                trans.Commit();
            }
            catch (NpgsqlException)
            {
                trans.Rollback();
            }
            finally
            {
                conn.Close();
            }
        }

        //Uppdatera databasens medlemstabell med parametrar
        public static void UpdateMedlem(string Medlemsnummer, string nyttFornamn, string nyttEfternamn, string nyGatuadress, string nyttPostnummer, string nyPostadress, string nyTelefon, string nyMailadress,
                                        int nyMedlemstyp, DateTime nyttStartDatum, DateTime nyttSlutDatum)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand(@"UPDATE medlem
                                                        SET fornamn = :nFornamn,
                                                            efternamn = :nEfternamn,
                                                            gatuadress = :nGatuadress,
                                                            postnummer = :nPostnummer,
                                                            postadress = :nPostadress,
                                                            telefonnummer = :nTelefon,
                                                            mailadress = :nMailadress,
                                                            medlemstyp = :nMedlemstyp,    
                                                            startdatum = :nStartdatum,
                                                            slutdatum = :nSlutdatum
                                                        WHERE medlemsnummer = :Medlemsnummer", conn);
             
            command.Parameters.Add(new NpgsqlParameter("Medlemsnummer", NpgsqlDbType.Varchar));
            command.Parameters["Medlemsnummer"].Value = Medlemsnummer;
            command.Parameters.Add(new NpgsqlParameter("nFornamn", NpgsqlDbType.Varchar));
            command.Parameters["nFornamn"].Value = nyttFornamn;
            command.Parameters.Add(new NpgsqlParameter("nEfternamn", NpgsqlDbType.Varchar));
            command.Parameters["nEfternamn"].Value = nyttEfternamn;
            command.Parameters.Add(new NpgsqlParameter("nGatuadress", NpgsqlDbType.Varchar));
            command.Parameters["nGatuadress"].Value = nyGatuadress;
            command.Parameters.Add(new NpgsqlParameter("nPostnummer", NpgsqlDbType.Varchar));
            command.Parameters["nPostnummer"].Value = nyttPostnummer;
            command.Parameters.Add(new NpgsqlParameter("nPostadress", NpgsqlDbType.Varchar));
            command.Parameters["nPostadress"].Value = nyPostadress; 
            command.Parameters.Add(new NpgsqlParameter("ntelefon", NpgsqlDbType.Varchar));
            command.Parameters["nTelefon"].Value = nyTelefon;
            command.Parameters.Add(new NpgsqlParameter("nMailadress", NpgsqlDbType.Varchar));
            command.Parameters["nMailadress"].Value = nyMailadress;
            command.Parameters.Add(new NpgsqlParameter("nMedlemstyp", NpgsqlDbType.Integer));
            command.Parameters["nMedlemstyp"].Value = nyMedlemstyp;
            command.Parameters.Add(new NpgsqlParameter("nStartdatum", NpgsqlDbType.Date));
            command.Parameters["nStartdatum"].Value = nyttStartDatum;
            command.Parameters.Add(new NpgsqlParameter("nSlutdatum", NpgsqlDbType.Date));
            command.Parameters["nSlutdatum"].Value = nyttSlutDatum;

            int numberOfRowsAffected = command.ExecuteNonQuery();

            conn.Close();
        }

        //Ta bort vald medlemsrad ur databasen
        public static void RemoveMedlem(string Medlemsnummer)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("DELETE FROM medlem WHERE medlemsnummer ='" + Medlemsnummer + "'", conn);
            int numberOfRowsAffected = command.ExecuteNonQuery();
            conn.Close();
        }

        //Hämta medlemmar, sortera efter förnamn och efternamn, placera i och returnera en Bindinglist
        public static BindingList<Medlem> GetMedlemmar()         //System ComponentModel Binding möjliggör uppdatering av listbox vid anrop
        {
            BindingList<Medlem> medlemslista = new BindingList<Medlem>();
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM medlem order by fornamn, efternamn", conn);
            NpgsqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Medlem medlem = new Medlem
                {
                    Medlemsnummer = (string)dr["medlemsnummer"],
                    Fornamn = (string)dr["fornamn"],
                    Efternamn = (string)dr["efternamn"],
                    Gatuadress = (string)dr["gatuadress"],
                    Postnummer = (string)dr["postnummer"],
                    Postadress = (string)dr["postadress"],
                    Telefonummer = (string)dr["telefonnummer"],
                    Mailadress = (string)dr["mailadress"], 
                    Medlemstyp = (int)dr["medlemstyp"],
                    Startdatum = (DateTime)dr["startdatum"],
                    Slutdatum = (DateTime)dr["slutdatum"]
                };
                medlemslista.Add(medlem);
            }
            conn.Close();

            return medlemslista;
        }

        //Söka medlem
        public static BindingList<Medlem> searchMedlemmar(string sokning)
        {
            BindingList<Medlem> soklista = new BindingList<Medlem>();
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM medlem WHERE medlemsnummer LIKE'" + sokning + "%' OR fornamn LIKE'" + sokning + "%' OR efternamn LIKE'" + sokning + "%' OR gatuadress LIKE'" + sokning + "%' OR postnummer LIKE'" + sokning + "%' OR postadress LIKE'" + sokning + "%' OR telefonnummer LIKE'" + sokning + "%' OR mailadress LIKE'" + sokning + "%'", conn);
            NpgsqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Medlem medlem = new Medlem
                {
                    Medlemsnummer = (string)dr["medlemsnummer"],
                    Fornamn = (string)dr["fornamn"],
                    Efternamn = (string)dr["efternamn"],
                    Gatuadress = (string)dr["gatuadress"],
                    Postnummer = (string)dr["postnummer"],
                    Postadress = (string)dr["postadress"],
                    Telefonummer = (string)dr["telefonnummer"],
                    Mailadress = (string)dr["mailadress"],
                    Medlemstyp = (int)dr["medlemstyp"],
                    Startdatum = (DateTime)dr["startdatum"],
                    Slutdatum = (DateTime)dr["slutdatum"]
                };
                soklista.Add(medlem);
            }
            conn.Close();

            return soklista;
        }
        //************************************************************************************************************************
        //*************************************************************************************************************************
        //Medlemskapstyp 

        public static void AddMedlemskapstyp(string nyMedlemskapstyp, string nyMedlemskapsnamn, int nyPris)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            NpgsqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                NpgsqlCommand command1 = new NpgsqlCommand(@"INSERT INTO medlemstyp (typ_av_medlemskap, medlemskapsnamn, pris)
                                                             VALUES (:addMedlemskapstyp, :addMedlemskapsnamn, :addPris)", conn);
                command1.Parameters.Add(new NpgsqlParameter("addMedlemskapstyp", NpgsqlDbType.Varchar));
                command1.Parameters["addMedlemskapstyp"].Value = nyMedlemskapstyp;
                command1.Parameters.Add(new NpgsqlParameter("addMedlemskapsnamn", NpgsqlDbType.Varchar));
                command1.Parameters["addMedlemskapsnamn"].Value = nyMedlemskapsnamn;
                command1.Parameters.Add(new NpgsqlParameter("addPris", NpgsqlDbType.Integer));
                command1.Parameters["addPris"].Value = nyPris;
                int numberOfRowsAffected = command1.ExecuteNonQuery();



                trans.Commit();
            }
            catch (NpgsqlException)
            {
                trans.Rollback();
            }
            finally
            {
                conn.Close();
            }
        }
        public static BindingList<Medlemtyp> HämtaMedlemstyp()
        {
            BindingList<Medlemtyp> medlemstypLista = new BindingList<Medlemtyp>();
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM medlemstyp ORDER BY typ_av_medlemskap, medlemskapsnamn", conn);
            NpgsqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Medlemtyp medlemstyp = new Medlemtyp
                {
                    MedlemstypId = (int)dr["medlemstyp_id"],
                    TypAvMedlemskap = (string)dr["typ_av_medlemskap"],
                    Medlemskapsnamn = (string)dr["medlemskapsnamn"],
                    Pris = (int)dr["pris"]

                };
                medlemstypLista.Add(medlemstyp);
            }
            conn.Close();

            return medlemstypLista;
        }
        public static void RemoveMedlemskapstyp(int MedlemskapstypID)
       {
           ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
           NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);                
           conn.Open();
           NpgsqlCommand command = new NpgsqlCommand("DELETE FROM medlemstyp WHERE medlemstyp_id ='" + MedlemskapstypID + "'", conn);
           int numberOfRowsAffected = command.ExecuteNonQuery();
           conn.Close();
       }

                
        public static BindingList<Medlemtyp> GetAktuellMedlemstyp(string aktuellMedlem)
        {
            BindingList<Medlemtyp> medlemstypLista = new BindingList<Medlemtyp>();
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT medlemstyp_id, typ_av_medlemskap, medlemskapsnamn, pris FROM medlemstyp, medlem WHERE medlem.medlemstyp = medlemstyp.medlemstyp_id AND medlem.medlemsnummer = '" + aktuellMedlem + "'", conn);
            NpgsqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Medlemtyp medlemstyp = new Medlemtyp
                {
                    MedlemstypId = (int)dr["medlemstyp_id"],
                    TypAvMedlemskap = (string)dr["typ_av_medlemskap"],
                    Medlemskapsnamn = (string)dr["medlemskapsnamn"],
                    Pris = (int)dr["pris"]

                };
                medlemstypLista.Add(medlemstyp);
            }
            conn.Close();

            return medlemstypLista;
        }

        //*************************************************************************************************************************************
        //*************************************************************************************************************************************
        //Tränare

        public static void AddInstruktor(string regInstruktorsnummer, string regFornamn, string regEfternamn, string regGatuadress, string regPostnummer, string regPostadress, string regTelefon, string regMailadress)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            NpgsqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                NpgsqlCommand command1 = new NpgsqlCommand(@"INSERT INTO instruktor (instruktorsnummer, fornamn, efternamn, mailadress, gatuadress, postnummer, postadress, telefonnummer)
                                                            VALUES (:newInstruktorsnummer, :newFornamn, :newEfternamn, :newMailadress, :newGatuadress, :newPostnummer, :newPostadress, :newTelefonnummer)", conn);

                command1.Parameters.Add(new NpgsqlParameter("newInstruktorsnummer", NpgsqlDbType.Varchar));
                command1.Parameters["newInstruktorsnummer"].Value = regInstruktorsnummer;
                command1.Parameters.Add(new NpgsqlParameter("newFornamn", NpgsqlDbType.Varchar));
                command1.Parameters["newFornamn"].Value = regFornamn;
                command1.Parameters.Add(new NpgsqlParameter("newEfternamn", NpgsqlDbType.Varchar));
                command1.Parameters["newEfternamn"].Value = regEfternamn;
                command1.Parameters.Add(new NpgsqlParameter("newGatuadress", NpgsqlDbType.Varchar));
                command1.Parameters["newGatuadress"].Value = regGatuadress;
                command1.Parameters.Add(new NpgsqlParameter("newPostnummer", NpgsqlDbType.Varchar));
                command1.Parameters["newPostnummer"].Value = regPostnummer;
                command1.Parameters.Add(new NpgsqlParameter("newPostadress", NpgsqlDbType.Varchar));
                command1.Parameters["newPostadress"].Value = regPostadress;      
                command1.Parameters.Add(new NpgsqlParameter("newTelefonnummer", NpgsqlDbType.Varchar));
                command1.Parameters["newTelefonnummer"].Value = regTelefon;
                command1.Parameters.Add(new NpgsqlParameter("newMailadress", NpgsqlDbType.Varchar));
                command1.Parameters["newMailadress"].Value = regMailadress;

                command1.Transaction = trans;
                int numberofAffectedRows = command1.ExecuteNonQuery();


                trans.Commit();
            }
            catch (NpgsqlException)                                          
            {
                trans.Rollback();
            }
            finally
            {
                conn.Close();
            }
        }

        //Updatera instruktörsuppgifter
        public static void UpdateInstruktor(string Instruktorsnummer, string nyttFornamn, string nyttEfternamn,  string nyGatuadress, string nyttPostnummer, string nyPostadress, string nyTelefon, string nyMailadress)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand(@"UPDATE instruktor
                                                       SET fornamn = :nFornamn,
                                                           efternamn = :nEfternamn,
                                                           gatuadress = :nGatuadress,
                                                           postnummer = :nPostnummer,
                                                           postadress = :nPostadress,
                                                           telefonnummer = :nTelefon,
                                                           mailadress = :nMailadress                                                    
                                                           WHERE instruktorsnummer = :Instruktorsnummer", conn);

            command.Parameters.Add(new NpgsqlParameter("instruktorsnummer", NpgsqlDbType.Varchar));
            command.Parameters["instruktorsnummer"].Value = Instruktorsnummer;
            command.Parameters.Add(new NpgsqlParameter("nFornamn", NpgsqlDbType.Varchar));
            command.Parameters["nFornamn"].Value = nyttFornamn;
            command.Parameters.Add(new NpgsqlParameter("nEfternamn", NpgsqlDbType.Varchar));
            command.Parameters["nEfternamn"].Value = nyttEfternamn;
            command.Parameters.Add(new NpgsqlParameter("nGatuadress", NpgsqlDbType.Varchar));
            command.Parameters["nGatuadress"].Value = nyGatuadress;
            command.Parameters.Add(new NpgsqlParameter("nPostnummer", NpgsqlDbType.Varchar));
            command.Parameters["nPostnummer"].Value = nyttPostnummer;
            command.Parameters.Add(new NpgsqlParameter("nPostadress", NpgsqlDbType.Varchar));
            command.Parameters["nPostadress"].Value = nyPostadress;
            command.Parameters.Add(new NpgsqlParameter("nTelefon", NpgsqlDbType.Varchar));
            command.Parameters["nTelefon"].Value = nyTelefon;
            command.Parameters.Add(new NpgsqlParameter("nMailadress", NpgsqlDbType.Varchar));
            command.Parameters["nMailadress"].Value = nyMailadress;


            int numberOfRowsAffected = command.ExecuteNonQuery();
            conn.Close();
        }

        //Ta bort rad från tabell tränare
        public static void RemoveInstruktor(string Instruktorsnummer)   // form1 614       
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("DELETE FROM instruktor WHERE instruktor.instruktorsnummer ='" + Instruktorsnummer + "'", conn);
            
            int numberOfRowsAffected = command.ExecuteNonQuery();
            conn.Close();
        }

        //Hämta, läs in, sortera & presentera instruktörslista
        public static BindingList<Instruktor> GetInstruktorer()                 
        {
            BindingList<Instruktor> instruktorslista = new BindingList<Instruktor>();
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM instruktor order by fornamn, efternamn", conn);
            NpgsqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Instruktor instruktor = new Instruktor
                {
                    Instruktorsnummer = (string)dr["instruktorsnummer"],
                    Fornamn = (string)dr["fornamn"],
                    Efternamn = (string)dr["efternamn"],
                    Gatuadress = (string)dr["gatuadress"],
                    Postnummer = (string)dr["postnummer"],
                    Postadress = (string)dr["postadress"],
                    Mailadress = (string)dr["mailadress"],
                    Telefonnummer = (string)dr["telefonnummer"]
                };
                instruktorslista.Add(instruktor);
            }
            conn.Close();

            return instruktorslista;
        }

        //Söka instruktör
        public static BindingList<Instruktor> searchInstruktorer(string sokning)
        {
            BindingList<Instruktor> soklista = new BindingList<Instruktor>();
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM instruktor WHERE instruktorsnummer LIKE'" + sokning + "%' OR fornamn LIKE'" + sokning + "%' OR efternamn LIKE'" + sokning + "%' OR gatuadress LIKE'" + sokning + "%' OR postnummer LIKE'" + sokning + "%' OR postadress LIKE'" + sokning + "%' OR mailadress LIKE'" + sokning + "%' OR telefonnummer LIKE'" + sokning + "%'", conn);
            NpgsqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Instruktor instruktor = new Instruktor
                {
                    Instruktorsnummer = (string)dr["instruktorsnummer"],
                    Fornamn = (string)dr["fornamn"],
                    Efternamn = (string)dr["efternamn"],
                    Gatuadress = (string)dr["gatuadress"],
                    Postnummer = (string)dr["postnummer"],
                    Postadress = (string)dr["postadress"],
                    Mailadress = (string)dr["mailadress"],
                    Telefonnummer = (string)dr["telefonnummer"]
                };
                soklista.Add(instruktor);
            }
            conn.Close();

            return soklista;
        }

        public static BindingList<Aktivitet> HämtaInstruktorsSchemalagdaPass(string instruktorsnummer)
        {
            BindingList<Aktivitet> passLista = new BindingList<Aktivitet>();
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM aktivitet WHERE ledande_instruktor = '" + instruktorsnummer + "'", conn);
            NpgsqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Aktivitet pass = new Aktivitet
                 {
                     Datum = (DateTime)dr["datum"],
                     Passnummer = (int)dr["passnummer"],
                     TidFrån = (DateTime)dr["tid_fran"],
                     TidTill = (DateTime)dr["tid_till"],
                     Ledande_Instruktor = (string)dr["ledande_instruktor"],
                     Traningstyp = (string)dr["traningstyp"],
                     Lokal = (int)dr["lokal"]
                 };
                passLista.Add(pass);
            }
            conn.Close();

            return passLista;
        }





        //Lägga till kompetensutbildning för vald instruktör
        public static void AddKompetens(string valdInstruktor, string kompetens)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            NpgsqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                NpgsqlCommand command1 = new NpgsqlCommand(@"INSERT INTO kompetens_instruktor (id_instruktor, id_kompetens)
                                                             VALUES (:addInstruktor, :addKompetens)", conn);
                command1.Parameters.Add(new NpgsqlParameter("addInstruktor", NpgsqlDbType.Varchar));
                command1.Parameters["addInstruktor"].Value = valdInstruktor;
                command1.Parameters.Add(new NpgsqlParameter("addKompetens", NpgsqlDbType.Varchar));
                command1.Parameters["addKompetens"].Value = kompetens;
                command1.Transaction = trans;
                int numberOfRowsAffected = command1.ExecuteNonQuery();


                trans.Commit();
            }
            catch (NpgsqlException)
            {
                trans.Rollback();
            }
            finally
            {
                conn.Close();
            }
        }
        //Hämta kompetenslista
        public static BindingList<Kompetens> HämtaKompetenser(string instruktorsnummer)
        {
            BindingList<Kompetens> kompetensLista = new BindingList<Kompetens>();
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM kompetens_instruktor WHERE id_instruktor = '" + instruktorsnummer + "'", conn);
            NpgsqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Kompetens kompetens = new Kompetens
                {
                    InstruktorsKompetens = (string)dr["id_kompetens"],
                    InstruktorsID = (string)dr["id_instruktor"]

                };
                kompetensLista.Add(kompetens);
            }
            conn.Close();

            return kompetensLista;
        }

        //************************************************************************************************************************
        //*************************************************************************************************************************
        //Bokningar

        public static void AddBokning(int bokAktivitet, string bokMedlem)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            NpgsqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                NpgsqlCommand command1 = new NpgsqlCommand(@"INSERT INTO bokning (id_passnummer, id_medlemsnummer)
                                                             VALUES (:bokaAktivitet, :bokaMedlem)", conn);
                command1.Parameters.Add(new NpgsqlParameter("bokaAktivitet", NpgsqlDbType.Integer));
                command1.Parameters["bokaAktivitet"].Value = bokAktivitet;
                command1.Parameters.Add(new NpgsqlParameter("bokaMedlem", NpgsqlDbType.Varchar));
                command1.Parameters["bokaMedlem"].Value = bokMedlem;
                command1.Transaction = trans;
                int numberOfRowsAffected = command1.ExecuteNonQuery();


                trans.Commit();
            }
            catch (NpgsqlException)
            {
                trans.Rollback();
            }
            finally
            {
                conn.Close();
            }
        }
        public static void RemoveBokning(int Passnummer, string Medlemsnummer)           
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("DELETE FROM bokning WHERE id_passnummer ='" + Passnummer + "' AND id_medlemsnummer ='" + Medlemsnummer + "'", conn);
            int numberOfRowsAffected = command.ExecuteNonQuery();
            conn.Close();
        }

        public static BindingList<Bokning> GetBokningar()         
        {
            BindingList<Bokning> bokningslista = new BindingList<Bokning>();
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM bokning order by id_passnummer, id_medlemsnummer", conn);
            NpgsqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Bokning bokning = new Bokning
                {
                    PassnummerID = (int)dr["id_passnummer"],
                    MedlemsID = (string)dr["id_medlemsnummer"]                    
                };
                bokningslista.Add(bokning);
            }
            conn.Close();

            return bokningslista;
        }
        //************************************************************************************************************************
        //*************************************************************************************************************************
        //Aktivitet
        public static void AddAktiviter(string regInstruktor, string regTraningstyp, int regLokal, DateTime regDatum, DateTime regTidFran, DateTime regTidTill, int regMaxantal) //DBF 719
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            NpgsqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                NpgsqlCommand command1 = new NpgsqlCommand(@"INSERT INTO aktivitet (ledande_instruktor, traningstyp, lokal, datum, tid_fran, tid_till, max_antal)
                                                             VALUES (:nInstruktor, :nTraningstyp, :nLokal, :nDatum, :nTid_fran, :nTid_till, :nMax_antal)", conn);
                command1.Parameters.Add(new NpgsqlParameter("nInstruktor", NpgsqlDbType.Varchar));
                command1.Parameters["nInstruktor"].Value = regInstruktor;
                command1.Parameters.Add(new NpgsqlParameter("nTraningstyp", NpgsqlDbType.Varchar));
                command1.Parameters["nTraningstyp"].Value = regTraningstyp;
                command1.Parameters.Add(new NpgsqlParameter("nLokal", NpgsqlDbType.Integer));
                command1.Parameters["nLokal"].Value = regLokal;
                command1.Parameters.Add(new NpgsqlParameter("nDatum", NpgsqlDbType.Date)); 
                command1.Parameters["nDatum"].Value = regDatum;
                command1.Parameters.Add(new NpgsqlParameter("nTid_fran", NpgsqlDbType.Time)); 
                command1.Parameters["nTid_fran"].Value = regTidFran;
                command1.Parameters.Add(new NpgsqlParameter("nTid_till", NpgsqlDbType.Time));
                command1.Parameters["nTid_till"].Value = regTidTill;
                command1.Parameters.Add(new NpgsqlParameter("nMax_antal", NpgsqlDbType.Integer));
                command1.Parameters["nMax_antal"].Value = regMaxantal;
                command1.Transaction = trans;
              
               int numberOfRowsAffected = command1.ExecuteNonQuery();
                trans.Commit();
            }
               
            catch (NpgsqlException)
            {
                trans.Rollback();
            }
            finally
            {
                conn.Close();
            }
        }
        public static BindingList<Medlem> HämtaMedlemsID(int passnummer)
        {
            BindingList<Medlem> Lista_medlemsId = new BindingList<Medlem>();
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM bokning, medlem WHERE bokning.id_medlemsnummer = medlem.medlemsnummer AND bokning.id_passnummer = '" + passnummer + "'", conn);
            NpgsqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Medlem medlem = new Medlem
                {
                    Medlemsnummer = (string)dr["medlemsnummer"],
                    Fornamn = (string)dr["fornamn"],
                    Efternamn = (string)dr["efternamn"],
                    Gatuadress = (string)dr["gatuadress"],
                    Postnummer = (string)dr["postnummer"],
                    Postadress = (string)dr["postadress"],
                    Telefonummer = (string)dr["telefonnummer"],
                    Mailadress = (string)dr["mailadress"],
                    Medlemstyp = (int)dr["medlemstyp"],
                    Startdatum = (DateTime)dr["startdatum"],
                    Slutdatum = (DateTime)dr["slutdatum"]

                };
                Lista_medlemsId.Add(medlem);
            }
            conn.Close();

            return Lista_medlemsId;
        }
        public static void RemovePass(int Passnummer) // form1 line 860
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();                                                     
            NpgsqlCommand command = new NpgsqlCommand("DELETE FROM aktivitet WHERE passnummer ='" + Passnummer + "'", conn);
            int numberOfRowsAffected = command.ExecuteNonQuery();
            conn.Close();
        }
        public static void UpdateAktivitet(string regInstruktor, string regTraningstyp, int regLokal, DateTime regDatum, DateTime regTidFran, DateTime regTidTill, int regMaxAntal, int regPassnummer)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand(@"UPDATE aktivitet
                                                        SET ledande_instruktor = :nInstruktor,
                                                            traningstyp = :nTraningstyp,
                                                            lokal = :nLokal,
                                                            datum = :nDatum,
                                                            tid_fran = :nTidFran,
                                                            tid_till = :nTidTill,
                                                            max_antal = :nMaxAntal
                                                        WHERE passnummer = :nPassnummer", conn);

            command.Parameters.Add(new NpgsqlParameter("nInstruktor", NpgsqlDbType.Varchar));
            command.Parameters["nInstruktor"].Value = regInstruktor;
            command.Parameters.Add(new NpgsqlParameter("nTraningstyp", NpgsqlDbType.Varchar));
            command.Parameters["nTraningstyp"].Value = regTraningstyp;
            command.Parameters.Add(new NpgsqlParameter("nLokal", NpgsqlDbType.Integer));
            command.Parameters["nLokal"].Value = regLokal;
            command.Parameters.Add(new NpgsqlParameter("nDatum", NpgsqlDbType.Date));
            command.Parameters["nDatum"].Value = regDatum;
            command.Parameters.Add(new NpgsqlParameter("nTidFran", NpgsqlDbType.Time));
            command.Parameters["nTidFran"].Value = regTidFran;
            command.Parameters.Add(new NpgsqlParameter("nTidTill", NpgsqlDbType.Time));
            command.Parameters["nTidTill"].Value = regTidTill;
            command.Parameters.Add(new NpgsqlParameter("nMaxAntal", NpgsqlDbType.Integer));
            command.Parameters["nMaxAntal"].Value = regMaxAntal;
            command.Parameters.Add(new NpgsqlParameter("nPassnummer", NpgsqlDbType.Integer));
            command.Parameters["nPassnummer"].Value = regPassnummer;

            command.ExecuteNonQuery();

            conn.Close();
        }
        public static BindingList<Aktivitet> GetAktiviteter1(DateTime mon, DateTime tue, DateTime wed, DateTime thu, DateTime fri, DateTime sat, DateTime sun)
        {
            BindingList<Aktivitet> aktivitetslista = new BindingList<Aktivitet>();
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM aktivitet WHERE datum = '" + mon.ToString("yyyy'-'MM'-'dd HH':'mm':'ss") + "' OR datum = '" + tue.ToString("yyyy'-'MM'-'dd HH':'mm':'ss") + "' OR datum ='" + wed.ToString("yyyy'-'MM'-'dd HH':'mm':'ss") + "' OR datum ='" + thu.ToString("yyyy'-'MM'-'dd HH':'mm':'ss") + "' OR datum = '" + fri.ToString("yyyy'-'MM'-'dd HH':'mm':'ss") + "' OR datum = '" + sat.ToString("yyyy'-'MM'-'dd HH':'mm':'ss") + "' OR datum = '" + sun.ToString("yyyy'-'MM'-'dd HH':'mm':'ss") + "' ORDER BY datum, tid_fran", conn); 
            NpgsqlDataReader dr = command.ExecuteReader();  
            while (dr.Read())
            {

                Aktivitet aktivitet = new Aktivitet
                {
                    Datum = (DateTime)dr["datum"],
                    Passnummer = (int)dr["passnummer"],
                    TidFrån = (DateTime)dr["tid_fran"],
                    TidTill = (DateTime)dr["tid_till"],
                    Ledande_Instruktor = (string)dr["ledande_instruktor"],
                    Traningstyp = (string)dr["traningstyp"],
                    Lokal = (int)dr["lokal"],
                    MaxAntal = (int)dr["max_antal"]  
                };
                aktivitetslista.Add(aktivitet);
            }
            conn.Close();

            return aktivitetslista;
        }

        public static BindingList<Aktivitet> GetAktiviteter2()
        {
            BindingList<Aktivitet> aktivitetslista = new BindingList<Aktivitet>();
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM aktivitet ORDER BY tid_fran, datum", conn);
            NpgsqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Aktivitet aktivitet = new Aktivitet
                {
                    Datum = (DateTime)dr["datum"],
                    Passnummer = (int)dr["passnummer"],
                    TidFrån = (DateTime)dr["tid_fran"],
                    TidTill = (DateTime)dr["tid_till"],
                    Ledande_Instruktor = (string)dr["ledande_instruktor"],
                    Traningstyp = (string)dr["traningstyp"],
                    Lokal = (int)dr["lokal"],
                    MaxAntal = (int)dr["max_antal"]
                };
                aktivitetslista.Add(aktivitet);
            }
            conn.Close();

            return aktivitetslista;
        }

        public static BindingList<Aktivitet> HämtaMedlemsBokning(string medlemsnummer)
        {
            BindingList<Aktivitet> ListaBokningMedlem = new BindingList<Aktivitet>();
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM aktivitet, bokning WHERE bokning.id_passnummer = aktivitet.passnummer AND bokning.id_medlemsnummer = '" + medlemsnummer + "'", conn);
            NpgsqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Aktivitet bokat = new Aktivitet
                 {
                     Datum = (DateTime)dr["datum"],
                     Passnummer = (int)dr["passnummer"],
                     TidFrån = (DateTime)dr["tid_fran"],
                     TidTill = (DateTime)dr["tid_till"],
                     Ledande_Instruktor = (string)dr["ledande_instruktor"],
                     Traningstyp = (string)dr["traningstyp"],
                     Lokal = (int)dr["lokal"]
                 };
                ListaBokningMedlem.Add(bokat);
            }
            conn.Close();

            return ListaBokningMedlem;
        }

        // Hämta träningstyp i combobox schema
        public static BindingList<Kompetens> GetTraningstyp()
        {
            BindingList<Kompetens> traningstyplista = new BindingList<Kompetens>();
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM kompetens_instruktor", conn);
            NpgsqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Kompetens traningstyp = new Kompetens
                {
                    InstruktorsKompetens = (string)dr["id_kompetens"],
                    InstruktorsID = (string)dr["id_instruktor"]
                };
                traningstyplista.Add(traningstyp);
            }
            conn.Close();

            return traningstyplista;
        }

        //***************************************************************************************************
        //LOKALER********************************************************************************************
        public static void AddLokal(int rumsnummer, string utrustning, string storlek)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            NpgsqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                NpgsqlCommand command1 = new NpgsqlCommand(@"INSERT INTO lokal (rumsnummer, utrustning, storlek)
                                                             VALUES (:nRumsnummer, :nUtrustning, :nStorlek)", conn);
                command1.Parameters.Add(new NpgsqlParameter("nUtrustning", NpgsqlDbType.Varchar));
                command1.Parameters["nUtrustning"].Value = utrustning;
                command1.Parameters.Add(new NpgsqlParameter("nStorlek", NpgsqlDbType.Varchar));
                command1.Parameters["nStorlek"].Value = storlek;
                command1.Parameters.Add(new NpgsqlParameter("nRumsnummer", NpgsqlDbType.Integer));
                command1.Parameters["nRumsnummer"].Value = rumsnummer;
                command1.Transaction = trans;

                int numberOfRowsAffected = command1.ExecuteNonQuery();
                trans.Commit();
            }

            catch (NpgsqlException)
            {
                trans.Rollback();
            }
            finally
            {
                conn.Close();
            }
        }
        // Hämta lokal i combobox schema
        public static BindingList<Lokal> GetLokal()
        {
            BindingList<Lokal> lokalLista = new BindingList<Lokal>();
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM lokal ORDER BY rumsnummer", conn);
            NpgsqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Lokal lokal = new Lokal
                {
                     Rumsnummer = (int)dr["rumsnummer"],
                     Utrustning = (string)dr["utrustning"],
                     Storlek = (string)dr["storlek"]
                };
                lokalLista.Add(lokal);
            }
            conn.Close();

            return lokalLista;
        }

        public static void RemoveLokal(int rumsnummer) 
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("DELETE FROM lokal WHERE rumsnummer ='" + rumsnummer + "'", conn);
            int numberOfRowsAffected = command.ExecuteNonQuery();
            conn.Close();
        }

        //FUnkar inte att converta till datetime eller int... ("SELECT * FROM aktivitet WHERE datum LIKE'" + Convert.ToDateTime(sokning) + "%' OR tid_fran LIKE'" + Convert.ToDateTime(sokning) + "%' OR ledande_instruktor LIKE'" + sokning + "%' OR traningstyp LIKE'" + sokning + "%' OR passnummer LIKE'" + Convert.ToInt16(sokning) + "'% OR lokal LIKE'" + Convert.ToInt16(sokning) + "%'", conn);
        public static BindingList<Aktivitet> searchAktivitet(string sokning)
        {
            BindingList<Aktivitet> soklista = new BindingList<Aktivitet>();
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM aktivitet WHERE ledande_instruktor LIKE'" + sokning + "%' OR traningstyp LIKE'" + sokning + "%'", conn);
            NpgsqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Aktivitet aktivitet = new Aktivitet
                {
                    Datum = (DateTime)dr["datum"],
                    Passnummer = (int)dr["passnummer"],
                    TidFrån = (DateTime)dr["tid_fran"],
                    TidTill = (DateTime)dr["tid_till"],
                    Ledande_Instruktor = (string)dr["ledande_instruktor"],
                    Traningstyp = (string)dr["traningstyp"],
                    Lokal = (int)dr["lokal"],
                    MaxAntal = (int)dr["max_antal"]       
                };
                soklista.Add(aktivitet);
            }
            conn.Close();

            return soklista ;
        }
        //Visa instruktörer vid vald träningstyp
        public static BindingList<Kompetens> HämtaInstruktorerMedVissKompetens(string träningstyp)
        {
            BindingList<Kompetens> kompetensLista = new BindingList<Kompetens>();
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT id_instruktor FROM kompetens_instruktor WHERE id_kompetens = '" + träningstyp + "'", conn);
            NpgsqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Kompetens kompetens = new Kompetens
                {
                    //InstruktorsKompetens = (string)dr["id_kompetens"],
                    InstruktorsID = (string)dr["id_instruktor"]                                        
                };
                kompetensLista.Add(kompetens);
            }
            conn.Close();

            return kompetensLista;
        }

        //************************************************************************************************************************
        //*************************************************************************************************************************
        //Användare
        //Skapar en användare med lösenord och ger access till olika delar i systemet beroende på vad de har för behörighet
        public static void CreateUser(string userName, string password, string userType)
        {             
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            NpgsqlTransaction trans = null;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                NpgsqlCommand command1 = new NpgsqlCommand(@"INSERT INTO inloggning (anvandarnamn, losenord, behorighet)
                                                             VALUES (:nyAnvandare, :nyttLosen, :nyBehorighet)", conn);
                command1.Parameters.Add(new NpgsqlParameter("nyAnvandare", NpgsqlDbType.Varchar));
                command1.Parameters["nyAnvandare"].Value = userName;
                command1.Parameters.Add(new NpgsqlParameter("nyttLosen", NpgsqlDbType.Varchar));
                command1.Parameters["nyttLosen"].Value = password;
                command1.Parameters.Add(new NpgsqlParameter("nyBehorighet", NpgsqlDbType.Varchar));
                command1.Parameters["nyBehorighet"].Value = userType;
                command1.Transaction = trans;
                int numberOfRowsAffected = command1.ExecuteNonQuery();

                NpgsqlCommand command2 = new NpgsqlCommand(@"SELECT behorighet
                                                             FROM inloggning 
                                                             WHERE anvandarnamn = :nAnvandare
                                                             AND losenord = :nLosen", conn);
                command2.Parameters.Add(new NpgsqlParameter("nAnvandare", NpgsqlDbType.Varchar));
                command2.Parameters["nAnvandare"].Value = userName;
                command2.Parameters.Add(new NpgsqlParameter("nLosen", NpgsqlDbType.Varchar));
                command2.Parameters["nLosen"].Value = password;
                command2.Transaction = trans;
                string behorighet = (string)command2.ExecuteScalar();

                trans.Commit();
            }
            catch (NpgsqlException)
            {
                trans.Rollback();
            }
            finally
            {
                conn.Close();
            }        
             
        }


        //************************************************************************************************************************
        //*************************************************************************************************************************
        //Logga in
        public static Inloggning GetLogin(string Anvandare, string Losen)   //kolla ifall loginet finns i databasen, isåfall returnera
        {
            Inloggning login = new Inloggning();
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM inloggning WHERE anvandarnamn = '" + Anvandare + "' AND losenord ='" + Losen + "'", conn);
            NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    Inloggning inlog = new Inloggning
                    {
                        Anvandarnamn = (string)dr["anvandarnamn"],
                        Losenord = (string)dr["losenord"],
                        Behorighet = (string)dr["behorighet"]                        
                    };
                    login = inlog;
                }
                conn.Close();
           
                return login;          
        }

        public static BindingList<Inloggning> GetUsers()  
        {
            BindingList<Inloggning> användarlista = new BindingList<Inloggning>();
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM inloggning ORDER BY anvandarnamn", conn);
            NpgsqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Inloggning användare = new Inloggning
                {
                    Anvandarnamn = (string)dr["anvandarnamn"],
                    Losenord = (string)dr["losenord"],
                    Behorighet = (string)dr["behorighet"]
                };
                användarlista.Add(användare);
            }
            conn.Close();

            return användarlista;
        }
        public static void DeleteUser(string userName)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("DELETE FROM inloggning WHERE anvandarnamn ='" + userName + "'", conn);
            int numberOfRowsAffected = command.ExecuteNonQuery();
            conn.Close();
        }
        public static void UpdateUser(string newAnvändare, string newLösenord, string newBehörighet, string aktuellAnvändare)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[conString];
            NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand(@"UPDATE inloggning
                                                        SET anvandarnamn = :nAnvandarnamn,
                                                            losenord = :nLosenord,
                                                            behorighet = :nBehorighet
                                                        WHERE anvandarnamn = :aAnvandare", conn);

            command.Parameters.Add(new NpgsqlParameter("nAnvandarnamn", NpgsqlDbType.Varchar));
            command.Parameters["nAnvandarnamn"].Value = newAnvändare;
            command.Parameters.Add(new NpgsqlParameter("nLosenord", NpgsqlDbType.Varchar));
            command.Parameters["nLosenord"].Value = newLösenord;
            command.Parameters.Add(new NpgsqlParameter("nBehorighet", NpgsqlDbType.Varchar));
            command.Parameters["nBehorighet"].Value = newBehörighet;
            command.Parameters.Add(new NpgsqlParameter("aAnvandare", NpgsqlDbType.Varchar));
            command.Parameters["aAnvandare"].Value = aktuellAnvändare;

            command.ExecuteNonQuery();

            conn.Close();
        }

              
    }
}
