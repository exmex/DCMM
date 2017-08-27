﻿using System.IO;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.LobbyServer
{
    public class UserInfoAnswerPacket : OutPacket
    {
        /// <summary>
        ///     The character count
        /// </summary>
        /// <remarks>Could use Characters.Length</remarks>
        public int CharacterCount;

        /// <summary>
        ///     The characters the user has
        /// </summary>
        public Character[] Characters;

        /// <summary>
        ///     The permissions the user has
        /// </summary>
        public int Permissions;

        /// <summary>
        ///     The username
        /// </summary>
        /// <remarks>Always 18</remarks>
        public string Username;

        public UserInfoAnswerPacket()
        {
            Permissions = 0;
            CharacterCount = 0;
            Username = "";
            Characters = new Character[0];
        }

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.UserInfoAck);
        }
        
        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Permissions);
                    bs.Write(CharacterCount);
                    bs.WriteUnicodeStatic(Username, 18);
                    bs.Write((long) 0);
                    bs.Write((long) 0);
                    bs.Write((long) 0);
                    bs.Write(0);

                    foreach (var character in Characters)
                    {
                        bs.WriteUnicodeStatic(character.Name, 21);
                        bs.Write(character.Cid);
                        bs.Write((int) character.Avatar);
                        bs.Write((int) character.Level);
                        bs.Write(character.CurrentCarId);
                        bs.Write(character.ActiveCar.CarType);
                        bs.Write(character.ActiveCar.BaseColor);
                        bs.Write(character.CreationDate);
                        bs.Write(character.Tid);
                        bs.Write(character.TeamMarkId);
                        bs.WriteUnicodeStatic(character.TeamName, 13);
                        bs.Write(0); // GuildType? (unsigned int nGuild;)
                    }
                }
                return ms.ToArray();
            }
        }
    }
}