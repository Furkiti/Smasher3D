//
//  TenjinUnityInterface.mm
//  Unity-iOS bridge
//
//  Created by Christopher Farm.
//
//

#include "TenjinUnityInterface.h"

extern "C" {
#define MALLOC_ARRAY(count, type) (count == 0 ? NULL : (type*) malloc(count * sizeof(type)))
void iosTenjin_InternalFreeStringStringKeyValuePairs(TenjinStringStringKeyValuePair* pairs, int32_t pairCount);
bool iosTenjin_InternalConvertDictionaryToStringStringPairs(NSDictionary<NSString*, NSObject*>* dictionary, TenjinStringStringKeyValuePair** outPairArray, int* outPairCount);

TenjinDeeplinkHandlerFunc registeredDeeplinkHandlerFunc;

void iosTenjinInit(const char* apiKey){
    NSString *apiKeyStr = [NSString stringWithUTF8String:apiKey];

    NSLog(@"Called Tenjin [TenjinSDK sharedInstanceWithToken:%@]", apiKeyStr);
    [TenjinSDK init:apiKeyStr];
}
    
void iosTenjinInitWithSharedSecret(const char* apiKey, const char* sharedSecret){
    NSString *apiKeyStr = [NSString stringWithUTF8String:apiKey];
    NSString *sharedSecretStr = [NSString stringWithUTF8String:sharedSecret];

    NSLog(@"Called Tenjin [TenjinSDK sharedInstanceWithToken:%@ sharedSecret:*]", apiKeyStr);
    [TenjinSDK init:apiKeyStr sharedSecret:sharedSecretStr];
}
    
void iosTenjinConnect(){
    [TenjinSDK connect];
}

void iosTenjinConnectWithDeferredDeeplink(const char* deferredDeeplink){
    NSString *deferredDeeplinkStr = [NSString stringWithUTF8String:deferredDeeplink];
    NSURL *deferredDeeplinkStrUri = [NSURL URLWithString:[deferredDeeplinkStr stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding]];
    NSLog(@"Called Tenjin [TenjinSDK connectWithDeferredDeeplink:%@]", deferredDeeplinkStr);

    [TenjinSDK connectWithDeferredDeeplink:deferredDeeplinkStrUri];
}

//void iosTenjinInitConnect(const char* apiKey){
//    NSString *apiKeyStr = [NSString stringWithUTF8String:apiKey];
//    
//    NSLog(@"Called Tenjin [TenjinSDK init:%@, + connect]", apiKeyStr);
//    [TenjinSDK init:apiKeyStr];
//    [TenjinSDK connect];
//}
//    
//void iosTenjinInitConnectWithSharedSecret(const char* apiKey, const char* sharedSecret){
//    NSString *apiKeyStr = [NSString stringWithUTF8String:apiKey];
//    NSString *sharedSecretStr = [NSString stringWithUTF8String:sharedSecret];
//    
//    NSLog(@"Called Tenjin [TenjinSDK init:%@ sharedSecret:*, + connect", apiKeyStr);
//    [TenjinSDK init:apiKeyStr sharedSecret:sharedSecretStr];
//    [TenjinSDK connect];
//}
//
//void iosTenjinInitConnectWithDeferredDeeplink(const char* apiKey, const char* deferredDeeplink){
//    NSString *apiKeyStr = [NSString stringWithUTF8String:apiKey];
//    NSString *deferredDeeplinkStr = [NSString stringWithUTF8String:deferredDeeplink];
//    NSURL *deferredDeeplinkStrUri = [NSURL URLWithString:[deferredDeeplinkStr stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding]];
//
//    NSLog(@"Called Tenjin [TenjinSDK init:%@, + connectWithDeferredDeeplink:%@]", apiKeyStr, deferredDeeplinkStr);
//    [TenjinSDK init:apiKeyStr];
//    [TenjinSDK connectWithDeferredDeeplink:deferredDeeplinkStrUri];
//}
//
//void iosTenjinInitConnectWithSharedSecretDeferredDeeplink(const char* apiKey, const char* sharedSecret, const char* deferredDeeplink){
//    NSString *apiKeyStr = [NSString stringWithUTF8String:apiKey];
//    NSString *sharedSecretStr = [NSString stringWithUTF8String:sharedSecret];
//    NSString *deferredDeeplinkStr = [NSString stringWithUTF8String:deferredDeeplink];
//    NSURL *deferredDeeplinkStrUri = [NSURL URLWithString:[deferredDeeplinkStr stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding]];
//    
//    NSLog(@"Called Tenjin [TenjinSDK init:%@ sharedSecret:*, + connectWithDeferredDeeplink:%@", apiKeyStr, deferredDeeplinkStr);
//    [TenjinSDK init:apiKeyStr sharedSecret:sharedSecretStr];
//    [TenjinSDK connectWithDeferredDeeplink:deferredDeeplinkStrUri];
//}
    
void iosTenjinSendEvent(const char* eventName){
    NSString *eventNameStr = [NSString stringWithUTF8String:eventName];
    NSLog(@"Called Tenjin [TenjinSDK sendEventWithName:%@]", eventNameStr);
    [TenjinSDK sendEventWithName:eventNameStr];
}

void iosTenjinSendEventWithValue(const char* eventName, const char* eventValue){
    NSString *eventNameStr = [NSString stringWithUTF8String:eventName];
    NSString *eventValueStr = [NSString stringWithUTF8String:eventValue];
    NSLog(@"Called Tenjin [TenjinSDK sendEventWithName:%@ andEventValue:%@]", eventNameStr, eventValueStr);
    [TenjinSDK sendEventWithName:eventNameStr andEventValue:eventValueStr];
}

void iosTenjinTransaction(const char* productId, const char* currencyCode, int quantity, double price){
    NSString *prodId = [NSString stringWithUTF8String:productId];
    NSString *curCode = [NSString stringWithUTF8String:currencyCode];
    NSDecimalNumber* pr = [[NSDecimalNumber alloc] initWithDouble:price];
    NSLog(@"Called Tenjin [TenjinSDK transactionWithProductName:%@ andCurrencyCode:%@ andQuantity:%d andUnitPrice:%f]", prodId, curCode, quantity, price);

    //call manual method in tenjin sdk
    [TenjinSDK transactionWithProductName:prodId andCurrencyCode:curCode andQuantity:quantity andUnitPrice:pr];
}

void iosTenjinTransactionWithReceiptData(const char* productId, const char* currencyCode, int quantity, double price, const char* transactionId, const char* receipt){
    NSString *prodId = [NSString stringWithUTF8String: productId];
    NSString *curCode = [NSString stringWithUTF8String: currencyCode];
    NSDecimalNumber *pr = [[NSDecimalNumber alloc] initWithDouble: price];
    NSString *tid = [NSString stringWithUTF8String: transactionId];
    NSString *rec = [NSString stringWithUTF8String: receipt];

    //call manual tenjin call with receipt data
    NSLog(@"Called Tenjin [TenjinSDK transactionWithProductName:%@ andCurrencyCode:%@ andQuantity:%d andUnitPrice:%f andTransactionId:%@ andBase64Receipt:%@]", prodId, curCode, quantity, price, tid, rec);
    [TenjinSDK transactionWithProductName: prodId andCurrencyCode:curCode andQuantity:quantity andUnitPrice:pr andTransactionId:tid andBase64Receipt:rec];
}
    
void iosTenjinOptIn(){
    NSLog(@"Called Tenjin [TenjinSDK optIn]");
    [TenjinSDK optIn];
}
    
void iosTenjinOptOut(){
    NSLog(@"Called Tenjin [TenjinSDK optOut]");
    [TenjinSDK optOut];
}
    
void iosTenjinOptInParams(char** params, int size){
    NSMutableArray *paramsList = [[NSMutableArray alloc] init];
    for (int i = 0; i < size; ++i) {
        NSString *str = [[NSString alloc] initWithCString:params[i] encoding:NSUTF8StringEncoding];
        NSLog(@"OptIn Param: %@", str);
        [paramsList addObject:str];
    }
    NSLog(@"Called Tenjin [TenjinSDK optInParams]");
    [TenjinSDK optInParams:paramsList];
}

void iosTenjinOptOutParams(char** params, int size){
    NSMutableArray *paramsList = [[NSMutableArray alloc] init];
    for (int i = 0; i < size; ++i) {
        NSString *str = [[NSString alloc] initWithCString:params[i] encoding:NSUTF8StringEncoding];
        NSLog(@"OptOut Param: %@", str);
        [paramsList addObject:str];
    }
    NSLog(@"Called Tenjin [TenjinSDK optOutParams]");
    [TenjinSDK optOutParams:paramsList];
}
    
void iosTenjinAppendAppSubversion(int subversion){
    NSNumber *subVersion = [NSNumber numberWithInt:subversion];
    NSLog(@"Called Tenjin [TenjinSDK appendAppSubversion]");
    [TenjinSDK appendAppSubversion:subVersion];
}
    
void iosTenjinRegisterDeepLinkHandler(TenjinDeeplinkHandlerFunc deeplinkHandlerFunc) {
    NSLog(@"Called iosTenjinRegisterDeepLinkHandler");
    registeredDeeplinkHandlerFunc = deeplinkHandlerFunc;
    [[TenjinSDK sharedInstance] registerDeepLinkHandler:^(NSDictionary *params, NSError *error) {
        NSLog(@"Entered deepLinkHandler");
        if (registeredDeeplinkHandlerFunc == NULL)
            return;

        TenjinStringStringKeyValuePair* deepLinkDataPairArray;
        int32_t deepLinkDataPairArrayCount;
        iosTenjin_InternalConvertDictionaryToStringStringPairs(params, &deepLinkDataPairArray, &deepLinkDataPairArrayCount);

        registeredDeeplinkHandlerFunc(deepLinkDataPairArray, deepLinkDataPairArrayCount);

        iosTenjin_InternalFreeStringStringKeyValuePairs(deepLinkDataPairArray, deepLinkDataPairArrayCount);
    }];
}

bool iosTenjin_InternalConvertDictionaryToStringStringPairs(NSDictionary<NSString*, NSObject*>* dictionary, TenjinStringStringKeyValuePair** outPairArray, int* outPairCount) {
    *outPairArray = NULL;
    *outPairCount = 0;
    if (dictionary == nil)
        return false;

    int pairCount = (int) dictionary.count;
    TenjinStringStringKeyValuePair* pairArray = MALLOC_ARRAY(pairCount, TenjinStringStringKeyValuePair);
    int counter = 0;
    for (NSString* key in dictionary) {
        NSObject* value = dictionary[key];
        TenjinStringStringKeyValuePair pair;
        pair.key = strdup([key UTF8String]);
        if ([value isKindOfClass:[NSNumber class]]) {
            NSNumber* numberValue = (NSNumber*) value;
            CFNumberType numberType = CFNumberGetType((CFNumberRef)numberValue);
            if (numberType == kCFNumberCharType) {
                pair.value = strdup([([numberValue boolValue] ? @"True" : @"False") UTF8String]);
            } else {
                pair.value = strdup([[numberValue stringValue] UTF8String]);
            }
            
        } else if ([value isKindOfClass:[NSString class]]) {
            pair.value = strdup([((NSString*) value) UTF8String]);
        } else {
            pair.value = strdup("Unknown data type");
        }

        pairArray[counter] = pair;
        counter++;
    }

    *outPairArray = pairArray;
    *outPairCount = pairCount;
    return true;
}

void iosTenjin_InternalFreeStringStringKeyValuePairs(TenjinStringStringKeyValuePair* pairs, int32_t pairCount) {
    for (int i = 0; i < pairCount; ++i) {
        free((void*) pairs[i].key);
        free((void*) pairs[i].value);
    }

    free((void*) pairs);
}
}
