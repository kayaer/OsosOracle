#include <p18f4520.h>
#include "card_driver.h"
#include "..\_delays.h"
#include "..\global.h"

#pragma udata access my_access

static near unsigned char read_data;
static near unsigned char card_buff[4];

void finish_card(void)
{
    CLK_OFF;
    RST_OFF;
    SET_IO_OUTPUT;
    SET_IO_LOW;
    CARD_VCC_OFF;
    RST_ON;
}

// kendileri cok hassas bir fonksiyondur, kurcalanmamasini tavsiye ederim :) mgocer...
static unsigned char card_write_byte(unsigned char data)
{
    unsigned char i,j;

    SET_IO_OUTPUT;
    SET_IO_LOW;
	delay10usx(36);

    if(KART_DISARDA) 
        return 1;
    
    j = 0;
    for(i = 8; i > 0; i--)
    {
		if(data & 0x01)
		{
          	SET_IO_HIGH;
			j++;
			Nop();Nop();Nop();
        }
		else
        {
            SET_IO_LOW;
            Nop();Nop();Nop();Nop();
		}
		
		if(KART_DISARDA) 
        {
//            finish_card(); 
            return 2;
        }
        
		data >>= 1;

		delay10usx(36);  // 360 us...
	}

	if(j & 0x01) 
	    SET_IO_HIGH;
	else  
	    SET_IO_LOW;
	    
	delay10usx(36);   

	SET_IO_HIGH;
    SET_IO_INPUT;

	delay10usx(36);  
	
	if(CARD_IO)
    {
   	    delay10usx(36);
   	    SET_IO_OUTPUT;
	}
    else 
        return 2;

   return 0;
}


static unsigned char card_read_byte(void)
{
    unsigned int i,j;
    
   	SET_IO_INPUT;

	for(i = 65535; i > 0; i--)
    {  
	    if(KART_DISARDA)
      	{
//      		finish_card();
            return 1;
      	}

		if (!CARD_IO)
    		break;
	}

	if(i == 0)
        return 1;

	delay10usx(18);    // 180 us...

	read_data = 0;
	j = 0;
	for(i = 8; i > 0; i--)
    {
		delay10usx(36);     // 360us...
		read_data >>= 1;
		
	    if (CARD_IO)
        {
			read_data |= 0x80;
			j++;
		}
        else
          	Nop();

    }
    
    delay10usx(36);

    if (CARD_IO)
	    j++;
   
    if (!(j & 0x01)) 
    {
   	    delay10usx(72);
    }

	return	0;
}


static unsigned char card_read_buff(unsigned char len)
{
    unsigned char i;
    
    if(len > 4)
        return 1;
        
    for(i = 0; i < len; i++)
    {
        if(card_read_byte())
            return 1;
        else
            card_buff[i] = read_data;    
    }
    
    return 0;   // okuma islemi basarili...   
}


unsigned char read_card(unsigned char adres,near unsigned char *pBuff)
{
    if(KART_DISARDA)
        return 3;

    delay1msx(2);

    card_write_byte(0x80);
    card_write_byte(0xBE);
    card_write_byte(0x00);
    card_write_byte(adres);
    card_write_byte(0x04);

    if(card_read_byte())        // okuma basarisiz ise...
        return 1;
        
    if(read_data != 0xBE)       // okuma hatali ise...    
        return 2;

    card_read_buff(4);
    *(long*)pBuff=*(long*)card_buff;

    card_read_buff(2);
    if((card_buff[0] != 0x90) || (card_buff[1] != 0x00))
        return 4;

    return 0;  // okuma islemi basarili...    
}


unsigned char update_card(unsigned char adres,near unsigned char *pBuff)
{
    unsigned char i;
    
    if(KART_DISARDA)
        return 3;

	delay1msx(3);

    card_write_byte(0x80);
    card_write_byte(0xDE);
    card_write_byte(0x00);
    card_write_byte(adres);
    card_write_byte(0x04);

    if(card_read_byte())    // okuma basarisiz ise...
        return 1;
        
    if(read_data != 0xDE)   // okuma hatali ise...
	    return 2;

    for(i = 0; i < 4; i++)
    {
        read_data = card_write_byte(pBuff[i]);
        if(read_data != 0)
            return 3;   // yazma hatasi ...      
    }
    
    card_read_buff(2);
    if((card_buff[0] != 0x90) || (card_buff[1] != 0x00))
        return 4;
    
    return 0;
}


unsigned char verify_card(unsigned char adres,near unsigned char *pBuff)
{
    unsigned char i;

    if(KART_DISARDA)
        return 3;
        
	delay1msx(3);

    card_write_byte(0x00);
    card_write_byte(0x20);
    card_write_byte(0x00);
    card_write_byte(adres);
    card_write_byte(0x04);

    if(card_read_byte())
        return 1;
        
    if(read_data != 0x20)
	    return 2;

    for(i = 0; i < 4; i++)
    {
        read_data = card_write_byte(pBuff[i]);
        if(read_data != 0)
            return 3;   // yazma hatasi ... 
    }

    card_read_buff(2);

    if((card_buff[0]!= 0x90) || (card_buff [1] !=0x00))
        return 4;
        
	return 0;
}


unsigned char start_card(void)
{
    unsigned int i;
    
	RST_OFF;
	CARD_VCC_ON;
    CLK_ON;
	SET_IO_INPUT;

	delay10msx_critical(10);
	RST_ON;


	for (i = 0; i < 4500; i++)
    {
        
	    if (!CARD_IO)
        {
            
		    card_read_buff(4);  // start_atr

			if ((card_buff[0] == 0x3b) && (card_buff[1] == 0x02) && (card_buff[2] == 0x53))// && (card_buff[3] == 1))
            {
				return 0;  // atr success
			}
            else
            {
//				finish_card();
//				delay10usx(30);
				return 1;   // atr error
			}   
			
		}
		
	}

//	CLK_OFF;
//	finish_card();
//	delay1msx(1);
	
	return 3; 
}
