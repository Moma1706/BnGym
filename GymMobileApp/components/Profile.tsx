import React, { useState } from 'react';
import { Text, View, TouchableOpacity, Image, StyleSheet } from 'react-native';
import { NavigationProp, ParamListBase } from '@react-navigation/native';
import Layout from './Layout';
import EncryptedStorage from 'react-native-encrypted-storage';
import axios from "axios";
import { useFocusEffect } from '@react-navigation/native';

type ProfileProps = {
  navigation: NavigationProp<ParamListBase>;
};

const Profile = ({ navigation }: ProfileProps) => {  
  const [userData, setUserData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    address: '',
    status: '',
    expiresOn: '',
    isFrozen: '',
    isInActive: ''
  });

  // useFocusEffect(
  //   React.useCallback(() => {
  //     const fetchData = async () => {
  //       const gymUserId = await EncryptedStorage.getItem('gym_user_id');
  //       // android:
  //       //  const response = await axios.get(`http://10.0.2.2:42068/api/App/${gymUserId}`);
  //       const response = await axios.get(`http://localhost:42068/api/App/${gymUserId}`);

  //       if (response.status === 200) {
  //         setUserData(response.data);
  //         userData.status = getStatusText();
  //       } else {
  //         // handle error
  //       }
  //     };

  //     fetchData();

  //     return () => {
  //       // Clean up the effect
  //     };
  //   }, [])
  // );

  const getStatusColor = () => {
    if (userData.isFrozen)
      return 'blue';
    else if (userData.isInActive)
      return 'red';
    
    return 'black';
  };
  
  const getStatusText = () => {
    if (userData.isFrozen)
      return 'ZALADJEN';
    else if (userData.isInActive)
      return 'NEAKTIVAN';

    return 'AKTIVAN';
  };

  const getExpiresOn = () => {
    return new Date(userData.expiresOn).toLocaleDateString('en-GB');
  };
  
  const handleLogout = async () => {
    // remove everything from localStorage
    await EncryptedStorage.clear();
    navigation.navigate('Login');
  };
  const handleChangePassword = () => {
    // open change password screen
    navigation.navigate('ChangePassword');
  };
  const handleUpdateProfile = () => {
    // open update user screen
    navigation.navigate('UpdateProfile');
  };
  
  return (
    <Layout navigation={navigation}>
      <View style={styles.container}>
        <View style={styles.top}>
          <Image
            source={require('../assets/black_image.jpeg')}
            style={styles.image}
          />
          <View style={{ marginLeft: 16 }}>
            <Text style={{ fontSize: 18, fontWeight: 'bold' }}>{userData.firstName + ' ' + userData.lastName}</Text>
            <Text style={styles.textTop}>{userData.email}</Text>
          </View>
        </View>

        <View style={{ marginTop: 24 }}>
          <Text style={{ fontSize: 16, fontWeight: 'bold', marginBottom: 8 }}>Adresa</Text>
          <Text style={styles.textTop}>{userData.address}</Text>
        </View>

        <View style={{ marginTop: 16 }}>
          <Text style={{ fontSize: 16, fontWeight: 'bold', marginBottom: 8 }}>Status</Text>
          <Text style={{ fontSize: 16, color: getStatusColor()}}>{getStatusText()}</Text>
        </View>

        <View style={{ marginTop: 16 }}>
          <Text style={{ fontSize: 16, fontWeight: 'bold', marginBottom: 8 }}>Važi do</Text>
          <Text style={styles.textTop}>{getExpiresOn()}</Text>
        </View>

        <View style={{ flexDirection: 'row', marginTop: 32 }} >
          <TouchableOpacity style={{ flex: 1, marginRight: 8 }} onPress={handleUpdateProfile} >
            <View style={{ backgroundColor: '#2c3e50', borderRadius: 5, alignItems: 'center',  padding: 16 }}>
              <Text style={{ color: 'white', fontSize: 16, textAlign: 'center' }}>Ažuriraj profil</Text>
            </View>
          </TouchableOpacity>

          <TouchableOpacity style={{ flex: 1, marginLeft: 8 }} onPress={handleChangePassword}>
            <View style={{ backgroundColor: '#2c3e50', borderRadius: 5, alignItems: 'center',  padding: 16 }}>
              <Text style={{ color: 'white', fontSize: 16, textAlign: 'center' }}>Promijeni lozinku</Text>
            </View>
          </TouchableOpacity>
        </View>

        <TouchableOpacity style={{ marginTop: 16 }} onPress={handleLogout}>
          <View style={{ backgroundColor: '#FF3B30', borderRadius: 16, padding: 16 }}>
            <Text style={{ color: 'white', fontSize: 16, textAlign: 'center' }}>Odjavi se</Text>
          </View>
        </TouchableOpacity>
      </View>
    </Layout>
    );
  };
  const styles = StyleSheet.create({
    container: {
      flex: 1,
      padding: 16
    },
    image: {
      width: 100,
      height: 100,
      borderRadius: 100
    },
    top: {
      flexDirection: 'row',
      alignItems: 'center'
    },
    textTop: {
      fontSize: 16

    },
    textLabels: {
      fontSize: 16,
      fontWeight: 'bold',
      marginBottom: 8
    }
  });

export default Profile;