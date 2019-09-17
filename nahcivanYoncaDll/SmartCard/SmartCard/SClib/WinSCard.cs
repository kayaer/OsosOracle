using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SCLibWin
{
    internal sealed class WinSCard
    {
        // Fields
        internal const uint IOCTL_SMARTCARD_CONFISCATE = 0x310010;
        internal const uint IOCTL_SMARTCARD_EJECT = 0x310018;
        internal const uint IOCTL_SMARTCARD_GET_ATTRIBUTE = 0x310008;
        internal const uint IOCTL_SMARTCARD_GET_LAST_ERROR = 0x31003c;
        internal const uint IOCTL_SMARTCARD_GET_PERF_CNTR = 0x310040;
        internal const uint IOCTL_SMARTCARD_GET_STATE = 0x310038;
        internal const uint IOCTL_SMARTCARD_IS_ABSENT = 0x31002c;
        internal const uint IOCTL_SMARTCARD_IS_PRESENT = 0x310028;
        internal const uint IOCTL_SMARTCARD_POWER = 0x310004;
        internal const uint IOCTL_SMARTCARD_READ = 0x310020;
        internal const uint IOCTL_SMARTCARD_SET_ATTRIBUTE = 0x31000c;
        internal const uint IOCTL_SMARTCARD_SET_PROTOCOL = 0x310030;
        internal const uint IOCTL_SMARTCARD_SWALLOW = 0x31001c;
        internal const uint IOCTL_SMARTCARD_TRANSMIT = 0x310014;
        internal const uint IOCTL_SMARTCARD_WRITE = 0x310024;
        internal const uint MAXIMUM_ATTR_STRING_LENGTH = 0x20;
        internal const uint SCARD_ABSENT = 1;
        internal const string SCARD_ALL_READERS = "SCard$AllReaders";
        internal const uint SCARD_ATTR_ATR_STRING = 0x90303;
        internal const uint SCARD_ATTR_CHANNEL_ID = 0x20110;
        internal const uint SCARD_ATTR_CHARACTERISTICS = 0x60150;
        internal const uint SCARD_ATTR_CURRENT_BWT = 0x80209;
        internal const uint SCARD_ATTR_CURRENT_CLK = 0x80202;
        internal const uint SCARD_ATTR_CURRENT_CWT = 0x8020a;
        internal const uint SCARD_ATTR_CURRENT_D = 0x80204;
        internal const uint SCARD_ATTR_CURRENT_EBC_ENCODING = 0x8020b;
        internal const uint SCARD_ATTR_CURRENT_F = 0x80203;
        internal const uint SCARD_ATTR_CURRENT_IFSC = 0x80207;
        internal const uint SCARD_ATTR_CURRENT_IFSD = 0x80208;
        internal const uint SCARD_ATTR_CURRENT_IO_STATE = 0x90302;
        internal const uint SCARD_ATTR_CURRENT_N = 0x80205;
        internal const uint SCARD_ATTR_CURRENT_PROTOCOL_TYPE = 0x80201;
        internal const uint SCARD_ATTR_CURRENT_W = 0x80206;
        internal const uint SCARD_ATTR_DEFAULT_CLK = 0x30121;
        internal const uint SCARD_ATTR_DEFAULT_DATA_RATE = 0x30123;
        internal const uint SCARD_ATTR_DEVICE_FRIENDLY_NAME = 0x7fff0003;
        internal const uint SCARD_ATTR_DEVICE_FRIENDLY_NAME_A = 0x7fff0003;
        internal const uint SCARD_ATTR_DEVICE_FRIENDLY_NAME_W = 0x7fff0005;
        internal const uint SCARD_ATTR_DEVICE_IN_USE = 0x7fff0002;
        internal const uint SCARD_ATTR_DEVICE_SYSTEM_NAME = 0x7fff0004;
        internal const uint SCARD_ATTR_DEVICE_SYSTEM_NAME_A = 0x7fff0004;
        internal const uint SCARD_ATTR_DEVICE_SYSTEM_NAME_W = 0x7fff0006;
        internal const uint SCARD_ATTR_DEVICE_UNIT = 0x7fff0001;
        internal const uint SCARD_ATTR_ESC_AUTHREQUEST = 0x7a005;
        internal const uint SCARD_ATTR_ESC_CANCEL = 0x7a003;
        internal const uint SCARD_ATTR_ESC_RESET = 0x7a000;
        internal const uint SCARD_ATTR_EXTENDED_BWT = 0x8020c;
        internal const uint SCARD_ATTR_ICC_INTERFACE_STATUS = 0x90301;
        internal const uint SCARD_ATTR_ICC_PRESENCE = 0x90300;
        internal const uint SCARD_ATTR_ICC_TYPE_PER_ATR = 0x90304;
        internal const uint SCARD_ATTR_MAX_CLK = 0x30122;
        internal const uint SCARD_ATTR_MAX_DATA_RATE = 0x30124;
        internal const uint SCARD_ATTR_MAX_IFSD = 0x30125;
        internal const uint SCARD_ATTR_MAXINPUT = 0x7a007;
        internal const uint SCARD_ATTR_POWER_MGMT_SUPPORT = 0x40131;
        internal const uint SCARD_ATTR_PROTOCOL_TYPES = 0x30120;
        internal const uint SCARD_ATTR_SUPRESS_T1_IFS_REQUEST = 0x7fff0007;
        internal const uint SCARD_ATTR_USER_AUTH_INPUT_DEVICE = 0x50142;
        internal const uint SCARD_ATTR_USER_TO_CARD_AUTH_DEVICE = 0x50140;
        internal const uint SCARD_ATTR_VENDOR_IFD_SERIAL_NO = 0x10103;
        internal const uint SCARD_ATTR_VENDOR_IFD_TYPE = 0x10101;
        internal const uint SCARD_ATTR_VENDOR_IFD_VERSION = 0x10102;
        internal const uint SCARD_ATTR_VENDOR_NAME = 0x10100;
        internal const uint SCARD_AUTOALLOCATE = uint.MaxValue;
        internal const uint SCARD_CONFISCATE_CARD = 4;
        internal const string SCARD_DEFAULT_READERS = "SCard$DefaultReaders";
        internal const uint SCARD_E_BAD_SEEK = 0x80100029;
        internal const uint SCARD_E_CANCELLED = 0x80100002;
        internal const uint SCARD_E_CANT_DISPOSE = 0x8010000e;
        internal const uint SCARD_E_CARD_UNSUPPORTED = 0x8010001c;
        internal const uint SCARD_E_CERTIFICATE_UNAVAILABLE = 0x8010002d;
        internal const uint SCARD_E_COMM_DATA_LOST = 0x8010002f;
        internal const uint SCARD_E_DIR_NOT_FOUND = 0x80100023;
        internal const uint SCARD_E_DUPLICATE_READER = 0x8010001b;
        internal const uint SCARD_E_FILE_NOT_FOUND = 0x80100024;
        internal const uint SCARD_E_ICC_CREATEORDER = 0x80100021;
        internal const uint SCARD_E_ICC_INSTALLATION = 0x80100020;
        internal const uint SCARD_E_INSUFFICIENT_BUFFER = 0x80100008;
        internal const uint SCARD_E_INVALID_ATR = 0x80100015;
        internal const uint SCARD_E_INVALID_CHV = 0x8010002a;
        internal const uint SCARD_E_INVALID_HANDLE = 0x80100003;
        internal const uint SCARD_E_INVALID_PARAMETER = 0x80100004;
        internal const uint SCARD_E_INVALID_TARGET = 0x80100005;
        internal const uint SCARD_E_INVALID_VALUE = 0x80100011;
        internal const uint SCARD_E_NO_ACCESS = 0x80100027;
        internal const uint SCARD_E_NO_DIR = 0x80100025;
        internal const uint SCARD_E_NO_FILE = 0x80100026;
        internal const uint SCARD_E_NO_KEY_CONTAINER = 0x80100030;
        internal const uint SCARD_E_NO_MEMORY = 0x80100006;
        internal const uint SCARD_E_NO_READERS_AVAILABLE = 0x8010002e;
        internal const uint SCARD_E_NO_SERVICE = 0x8010001d;
        internal const uint SCARD_E_NO_SMARTCARD = 0x8010000c;
        internal const uint SCARD_E_NO_SUCH_CERTIFICATE = 0x8010002c;
        internal const uint SCARD_E_NOT_READY = 0x80100010;
        internal const uint SCARD_E_NOT_TRANSACTED = 0x80100016;
        internal const uint SCARD_E_PCI_TOO_SMALL = 0x80100019;
        internal const uint SCARD_E_PROTO_MISMATCH = 0x8010000f;
        internal const uint SCARD_E_READER_UNAVAILABLE = 0x80100017;
        internal const uint SCARD_E_READER_UNSUPPORTED = 0x8010001a;
        internal const uint SCARD_E_SERVER_TOO_BUSY = 0x80100031;
        internal const uint SCARD_E_SERVICE_STOPPED = 0x8010001e;
        internal const uint SCARD_E_SHARING_VIOLATION = 0x8010000b;
        internal const uint SCARD_E_SYSTEM_CANCELLED = 0x80100012;
        internal const uint SCARD_E_TIMEOUT = 0x8010000a;
        internal const uint SCARD_E_UNEXPECTED = 0x8010001f;
        internal const uint SCARD_E_UNKNOWN_CARD = 0x8010000d;
        internal const uint SCARD_E_UNKNOWN_READER = 0x80100009;
        internal const uint SCARD_E_UNKNOWN_RES_MNG = 0x8010002b;
        internal const uint SCARD_E_UNSUPPORTED_FEATURE = 0x80100022;
        internal const uint SCARD_E_WRITE_TOO_MANY = 0x80100028;
        internal const uint SCARD_EJECT_CARD = 3;
        internal const uint SCARD_F_COMM_ERROR = 0x80100013;
        internal const uint SCARD_F_INTERNAL_ERROR = 0x80100001;
        internal const uint SCARD_F_UNKNOWN_ERROR = 0x80100014;
        internal const uint SCARD_F_WAITED_TOO_LONG = 0x80100007;
        internal const uint SCARD_LEAVE_CARD = 0;
        internal const string SCARD_LOCAL_READERS = "SCard$LocalReaders";
        internal const uint SCARD_NEGOTIABLE = 5;
        internal const uint SCARD_P_SHUTDOWN = 0x80100018;
        internal const uint SCARD_PERF_BYTES_TRANSMITTED = 0x7ffe0002;
        internal const uint SCARD_PERF_NUM_TRANSMISSIONS = 0x7ffe0001;
        internal const uint SCARD_PERF_TRANSMISSION_TIME = 0x7ffe0003;
        internal const uint SCARD_POWERED = 4;
        internal const uint SCARD_PRESENT = 2;
        internal const uint SCARD_PROTOCOL_DEFAULT = 0x80000000;
        internal const uint SCARD_PROTOCOL_OPTIMAL = 0;
        internal const uint SCARD_PROTOCOL_RAW = 0x10000;
        internal const uint SCARD_PROTOCOL_T0 = 1;
        internal const uint SCARD_PROTOCOL_T1 = 2;
        internal const uint SCARD_PROTOCOL_Tx = 3;
        internal const uint SCARD_PROTOCOL_UNDEFINED = 0;
        internal const uint SCARD_READER_CONFISCATES = 4;
        internal const uint SCARD_READER_EJECTS = 2;
        internal const uint SCARD_READER_SWALLOWS = 1;
        internal const uint SCARD_READER_TYPE_IDE = 0x10;
        internal const uint SCARD_READER_TYPE_KEYBOARD = 4;
        internal const uint SCARD_READER_TYPE_PARALELL = 2;
        internal const uint SCARD_READER_TYPE_PCMCIA = 0x40;
        internal const uint SCARD_READER_TYPE_SCSI = 8;
        internal const uint SCARD_READER_TYPE_SERIAL = 1;
        internal const uint SCARD_READER_TYPE_USB = 0x20;
        internal const uint SCARD_READER_TYPE_VENDOR = 240;
        internal const uint SCARD_RESET_CARD = 1;
        internal const uint SCARD_S_SUCCESS = 0;
        internal const uint SCARD_SCOPE_SYSTEM = 2;
        internal const uint SCARD_SCOPE_TERMINAL = 1;
        internal const uint SCARD_SCOPE_USER = 0;
        internal const uint SCARD_SHARE_DIRECT = 3;
        internal const uint SCARD_SHARE_EXCLUSIVE = 1;
        internal const uint SCARD_SHARE_SHARED = 2;
        internal const uint SCARD_SPECIFIC = 6;
        internal const uint SCARD_STATE_ATRMATCH = 0x40;
        internal const uint SCARD_STATE_CHANGED = 2;
        internal const uint SCARD_STATE_EMPTY = 0x10;
        internal const uint SCARD_STATE_EXCLUSIVE = 0x80;
        internal const uint SCARD_STATE_IGNORE = 1;
        internal const uint SCARD_STATE_INUSE = 0x100;
        internal const uint SCARD_STATE_MUTE = 0x200;
        internal const uint SCARD_STATE_PRESENT = 0x20;
        internal const uint SCARD_STATE_UNAVAILABLE = 8;
        internal const uint SCARD_STATE_UNAWARE = 0;
        internal const uint SCARD_STATE_UNKNOWN = 4;
        internal const uint SCARD_STATE_UNPOWERED = 0x400;
        internal const uint SCARD_SWALLOWED = 3;
        internal const string SCARD_SYSTEM_READERS = "SCard$SystemReaders";
        internal const uint SCARD_UNKNOWN = 0;
        internal const uint SCARD_UNPOWER_CARD = 2;
        internal const uint SCARD_W_CANCELLED_BY_USER = 0x8010006e;
        internal const uint SCARD_W_CARD_NOT_AUTHENTICATED = 0x8010006f;
        internal const uint SCARD_W_CHV_BLOCKED = 0x8010006c;
        internal const uint SCARD_W_EOF = 0x8010006d;
        internal const uint SCARD_W_REMOVED_CARD = 0x80100069;
        internal const uint SCARD_W_RESET_CARD = 0x80100068;
        internal const uint SCARD_W_SECURITY_VIOLATION = 0x8010006a;
        internal const uint SCARD_W_UNPOWERED_CARD = 0x80100067;
        internal const uint SCARD_W_UNRESPONSIVE_CARD = 0x80100066;
        internal const uint SCARD_W_UNSUPPORTED_CARD = 0x80100065;
        internal const uint SCARD_W_WRONG_CHV = 0x8010006b;
        public const long FILE_DEVICE_SMARTCARD = 0x310000; // Reader action IOCTLs
        public const long IOCTL_SMARTCARD_ACR128_ESCAPE_COMMAND = FILE_DEVICE_SMARTCARD + 2079 * 4;


        // Methods
        [DllImport("winscard.dll")]
        internal static extern uint SCardBeginTransaction(IntPtr hCard);
        [DllImport("winscard.dll")]
        internal static extern uint SCardCancel(IntPtr hContext);
        [DllImport("winscard.dll", CharSet = CharSet.Auto)]
        internal static extern uint SCardConnect(IntPtr hContext, string szReader, uint dwShareMode, uint dwPreferredProtocols, out IntPtr phCard, out uint pdwActiveProtocol);
        [DllImport("winscard.dll")]
        internal static extern uint SCardControl(IntPtr hCard, uint dwControlCode, [In] byte[] lpInBuffer, uint nInBufferSize, [In, Out] byte[] lpOutBuffer, uint nOutBufferSize, out uint lpBytesReturned);
        [DllImport("winscard.dll")]
        internal static extern uint SCardDisconnect(IntPtr hCard, uint dwDisposition);
        [DllImport("winscard.dll")]
        internal static extern uint SCardEndTransaction(IntPtr hCard, uint dwDisposition);
        [DllImport("winscard.dll")]
        internal static extern uint SCardEstablishContext(uint dwScope, IntPtr pvReserved1, IntPtr pvReserved2, out IntPtr phContext);
        [DllImport("winscard.dll")]
        internal static extern uint SCardFreeMemory(IntPtr hContext, IntPtr pvMem);
        [DllImport("winscard.dll")]
        internal static extern uint SCardGetAttrib(IntPtr hCard, uint dwAttrId, ref IntPtr pbAttr, ref uint pcbAttrLen);
        [DllImport("winscard.dll", CharSet = CharSet.Auto)]
        internal static extern uint SCardGetStatusChange(IntPtr hContext, uint dwTimeout, [In, Out] SCARD_READERSTATE[] rgReaderStates, uint cReaders);
        [DllImport("winscard.dll")]
        internal static extern uint SCardIsValidContext(IntPtr hContext);
        [DllImport("winscard.dll", CharSet = CharSet.Auto)]
        internal static extern uint SCardListReaderGroups(IntPtr hContext, ref IntPtr pmszGroups, ref uint pcchGroups);
        [DllImport("winscard.dll", CharSet = CharSet.Auto)]
        internal static extern uint SCardListReaders(IntPtr hContext, string mszGroups, ref IntPtr pmszReaders, ref uint pcchReaders);
        [DllImport("winscard.dll")]
        internal static extern uint SCardReconnect(IntPtr hCard, uint dwShareMode, uint dwPreferredProtocols, uint dwInitialization, out uint pdwActiveProtocol);
        [DllImport("winscard.dll")]
        internal static extern uint SCardReleaseContext(IntPtr hContext);
        [DllImport("winscard.dll")]
        internal static extern uint SCardSetAttrib(IntPtr hCard, uint dwAttrId, [In] byte[] pbAttr, uint cbAttrLen);
        [DllImport("winscard.dll", CharSet = CharSet.Auto)]
        internal static extern uint SCardStatus(IntPtr hCard, StringBuilder szReaderName, ref uint pcchReaderLen, out uint pdwState, out uint pdwProtocol, [Out] byte[] pbAtr, ref uint pcbAtrLen);
        [DllImport("winscard.dll")]
        internal static extern uint SCardTransmit(IntPtr hCard, SCARD_IO_REQUEST pioSendPci, [In] byte[] pbSendBuffer, uint cbSendLength, SCARD_IO_REQUEST pioRecvPci, [In, Out] byte[] pbRecvBuffer, ref uint pcbRecvLength);

        // Nested Types
        [StructLayout(LayoutKind.Sequential)]
        internal class SCARD_IO_REQUEST
        {
            internal uint dwProtocol = 0;
            internal uint cbPciLength;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct SCARD_READERSTATE
        {
            internal string szReader;
            internal IntPtr pvUserData;
            internal uint dwCurrentState;
            internal uint dwEventState;
            internal uint cbAtr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x24, ArraySubType = UnmanagedType.U1)]
            internal byte[] rgbAtr;
        }
    }


}
