//
//  TenjinUnityInterface.h
//  Unity-iOS bridge
//
//  Created by Christopher Farm.
//
//

#ifndef __Unity_iPhone__TenjinUnityInterface__
#define __Unity_iPhone__TenjinUnityInterface__

#include "TenjinSDK.h"

extern "C" {
    
typedef struct TenjinStringStringKeyValuePair {
    const char* key;
    const char* value;
} TenjinStringStringKeyValuePair;

typedef void (*TenjinDeeplinkHandlerFunc)(TenjinStringStringKeyValuePair* deepLinkDataPairArray, int32_t deepLinkDataPairCount);

void iosTenjinInit(const char* apiKey);
void iosTenjinInitWithSharedSecret(const char* apiKey, const char* sharedSecret);
void iosTenjinConnect();
void iosTenjinConnectWithDeferredDeeplink(const char* deferredDeeplink);
    
//void iosTenjinInitConnect(const char* apiKey);
//void iosTenjinInitConnectWithSharedSecret(const char* apiKey, const char* sharedSecret);
//void iosTenjinInitConnectWithDeferredDeeplink(const char* apiKey, const char* deferredDeeplink);
//void iosTenjinInitConnectWithSharedSecretDeferredDeeplink(const char* apiKey, const char* sharedSecret, const char* deferredDeeplink);
    
void iosTenjinSendEvent(const char* eventName);
void iosTenjinSendEventWithValue(const char* eventName, const char* eventValue);
void iosTenjinTransaction(const char* productId, const char* currencyCode, int quantity, double price);
void iosTenjinTransactionWithReceiptData(const char* productId, const char* currencyCode, int quantity, double price, const char* transactionId, const char* receipt);
void iosTenjinRegisterDeepLinkHandler(TenjinDeeplinkHandlerFunc deeplinkHandlerFunc);

void iosTenjinOptIn();
void iosTenjinOptOut();
void iosTenjinOptInParams(char** params, int size);
void iosTenjinOptOutParams(char** params, int size);
void iosTenjinAppendAppSubversion(int subversion);

}
#endif
