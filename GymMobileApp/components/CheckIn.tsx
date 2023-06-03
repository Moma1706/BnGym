import React, { useState, useEffect, useRef } from 'react';
import { StyleSheet, Alert, ActivityIndicator, View, Text, StatusBar } from 'react-native';
import { Camera, Frame, useCameraDevices, useFrameProcessor } from 'react-native-vision-camera';
import Layout from './Layout';
import { NavigationProp, ParamListBase } from '@react-navigation/native';
import axios from "axios";
import { BarcodeFormat, useScanBarcodes } from 'vision-camera-code-scanner';

type CheckInProps = {
  navigation: NavigationProp<ParamListBase>;
};

const CheckIn = ({ navigation }: CheckInProps) => {
  const camera = useRef<Camera>(null);
  const devices = useCameraDevices('wide-angle-camera');
  const device = devices.back;
  const [frameProcessor, barcodes] = useScanBarcodes([
    BarcodeFormat.ALL_FORMATS, // You can only specify a particular format
  ]);

const [barcode, setBarcode] = React.useState('');
const [isScanned, setIsScanned] = React.useState(false);

  // const frameProcessor = useFrameProcessor((frame) => {
  //   console.log(frame);
  //   const scannedQRCode = frame.toString();
  //   const expectedQRCode = 'check-in-process'; // Replace with your expected QR code
  //   if (scannedQRCode !== expectedQRCode) {
  //     Alert.alert('Error', 'Invalid QR code.');
  //     return;
  //   }

  //   Alert.alert('Dobar code');
    // const gymUserId = 'YOUR_ENCRYPTED_STORAGE_ID';
    // const url = `/api/App/check-in/${gymUserId}`;
    // fetch(url, {
    //   method: 'POST',
    // })
    //   .then(response => response.json())
    //   .then(data => {
    //     Alert.alert('Success', 'Successfully checked in.');
    //   })
    //   .catch(error => {
    //     Alert.alert('Error', 'An error occurred while checking in.');
    //   });
  // }, [])

  // function scanQRCodes(frame: Frame): string[] {
  //   'worklet'
  //   return __scanCodes(frame)
  // }

  // const frameProcessor = useFrameProcessor((frame) => {
  //   'worklet'
  //   console.log(frame);
  //   const qrCodes = scanQRCodes(frame)
  //   if (qrCodes.length > 0) {
  //     console.log('codeee: ', qrCodes);
  //     // runOnJS(onQRCodeDetected)(qrCodes[0])
  //   //  console.log(qrCodes[0]);
  //   }
  // }, [])

  React.useEffect(() => {
    toggleActiveState();
    return () => {
      barcodes;
    };
  }, [barcodes]);

const toggleActiveState = async () => {
    if (barcodes && barcodes.length > 0 && isScanned === false) {
      setIsScanned(true);
      // setBarcode('');
      barcodes.forEach(async (scannedBarcode: any) => {
        if (scannedBarcode.rawValue !== '') {
          console.log('Ima li te: ',scannedBarcode.rawValue )
          setBarcode(scannedBarcode.rawValue);
          Alert.alert(barcode);
        }
      });
    }
  };

  if (device == null)
    return ( 
    // <Layout navigation={navigation}>
    //   <ActivityIndicator size='large'/>
    // </Layout>
    <View style={styles.box1}/>
    );

  return (
    <Layout navigation={navigation}>
      {/* <View style={styles.box}>
      <Camera
          style={StyleSheet.absoluteFill}
          device={device}
          isActive={true}
          frameProcessor={frameProcessor}
           frameProcessorFps={5}
          video = {true}
          audio = {true}
        />
      </View> */}
        <>
        <StatusBar barStyle="light-content" backgroundColor="#000000" />
        <Camera
          style={StyleSheet.absoluteFill}
          device={device}
          isActive={!isScanned}
          frameProcessor={frameProcessor}
          frameProcessorFps={5}
          audio={false}
        />
       {/* <RNHoleView
            holes={[
              {
                x: widthToDp('8.5%'),
                y: heightToDp('36%'),
                width: widthToDp('83%'),
                height: heightToDp('20%'),
                borderRadius: 10,
              },
            ]}
            style={styles.rnholeView}
          /> */}
      </>
    </Layout>
  );
}

const styles = StyleSheet.create({
  box: {
    width: '100%',
    height: '100%',
  },

  box1: {
    width: 300,
    height: 200,
    backgroundColor: 'green'
  },
  barcodeTextURL: {
    fontSize: 20,
    color: 'white',
    fontWeight: 'bold',
  },
  button: {
    backgroundColor: 'blue',
    padding: 10,
    borderRadius: 5,
    alignItems: 'center',
  },
  buttonText: {
    color: 'white',
    fontWeight: 'bold',
  },
  camera: {
    flex: 1,
  },
});

export default CheckIn;
