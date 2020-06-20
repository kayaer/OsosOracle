using System;
using System.Collections.Generic;
using System.Text;

namespace Listener.Helpers
{
    public class MasterKey
    {
        private byte[] mkey1 = new byte[8] { 13, 72, 203, 149, 54, 215, 70, 138 };
        private byte[] mkey2 = new byte[8] { 80, 156, 93, 172, 45, 250, 21, 189 };
        private byte[] mkey3 = new byte[8] { 0, 98, 188, 146, 15, 233, 31, 44 };

        public MasterKey()
        {

        }
        public MasterKey(byte[] keyDizi)
        {
            Buffer.BlockCopy(keyDizi, 0, mkey1, 0, 8);
            Buffer.BlockCopy(keyDizi, 8, mkey2, 0, 8);
            Buffer.BlockCopy(keyDizi, 16, mkey3, 0, 8);
        }
        public byte[] Mkey1
        {
            get { return mkey1; }
            set { mkey1 = value; }
        }
        public byte[] Mkey2
        {
            get { return mkey2; }
            set { mkey2 = value; }
        }
        public byte[] Mkey3
        {
            get { return mkey3; }
            set { mkey3 = value; }
        }
        //public byte[] Mkey1
        //{
        //    get { return mkey1; }
        //}
        //public byte[] Mkey2
        //{
        //    get { return mkey2; }
        //}
        //public byte[] Mkey3
        //{
        //    get { return mkey3; }
        //}

        public byte[] getKey()
        {
            byte[] key = new byte[24];
            Buffer.BlockCopy(mkey1, 0, key, 0, 8);
            Buffer.BlockCopy(mkey2, 0, key, 8, 8);
            Buffer.BlockCopy(mkey3, 0, key, 16, 8);
            return key;
        }
        public byte[] getKey(byte[] key1)
        {
            byte[] key = new byte[24];
            Buffer.BlockCopy(key1, 0, key, 0, 8);
            Buffer.BlockCopy(mkey2, 0, key, 8, 8);
            Buffer.BlockCopy(mkey3, 0, key, 16, 8);
            return key;
        }
        public byte[] getKey(byte[] key1, byte[] key2)
        {
            byte[] key = new byte[24];
            Buffer.BlockCopy(key1, 0, key, 0, 8);
            Buffer.BlockCopy(key2, 0, key, 8, 8);
            Buffer.BlockCopy(mkey3, 0, key, 16, 8);
            return key;
        }
    }
}
