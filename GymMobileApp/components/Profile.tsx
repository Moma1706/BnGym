import React, { useState } from 'react';
import { Text, View, TouchableOpacity, Image, StyleSheet, Alert } from 'react-native';
import { NavigationProp, ParamListBase } from '@react-navigation/native';
import Layout from './Layout';
import EncryptedStorage from 'react-native-encrypted-storage';
import axios from "axios";
import { useFocusEffect } from '@react-navigation/native';
import { BASE_URL } from '../config/api-url.config';

type ProfileProps = {
  navigation: NavigationProp<ParamListBase>;
};

const Profile = ({ navigation }: ProfileProps) => {  
  const profileUrl = `${BASE_URL}/App`;
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

  useFocusEffect(
    React.useCallback(() => {
      const fetchData = async () => {
        const gymUserId = await EncryptedStorage.getItem('gym_user_id');
        // send API request
        try {
          const response = await axios.get(`${profileUrl}/${gymUserId}`);
  
          // API return 200 OK
          if (response.status === 200) {
            setUserData(response.data);
            userData.status = getStatusText();
          } else
            Alert.alert(response.data.message)
        } catch (error: any) {
          console.log(error.response)
          if (error.response && (error.response.status === 400 || error.response.status === 401))
            Alert.alert(error.response.data.error);
          else
            Alert.alert("Došlo je do greške prilikom dobijanja podataka");
        }
      };

      fetchData();
      return () => {
        // Clean up the effect
      };
    }, [])
  );

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
            source={require('../assets/logo.png')}
            style={styles.image}
          />
          <View style={{ marginLeft: 16 }}>
            <Text style={{ fontSize: 18, fontWeight: 'bold', color: 'black' }}>{userData.firstName + ' ' + userData.lastName}</Text>
            <Text style={styles.textTop}>{userData.email}</Text>
          </View>
        </View>

        <View style={{ marginTop: 24 }}>
          <Text style={{ fontSize: 16, fontWeight: 'bold', marginBottom: 8, color: 'black'  }}>Adresa</Text>
          <Text style={styles.textTop}>{userData.address}</Text>
        </View>

        <View style={{ marginTop: 16 }}>
          <Text style={{ fontSize: 16, fontWeight: 'bold', marginBottom: 8, color: 'black'  }}>Status</Text>
          <Text style={{ fontSize: 16, color: getStatusColor()}}>{getStatusText()}</Text>
        </View>

        <View style={{ marginTop: 16 }}>
          <Text style={{ fontSize: 16, fontWeight: 'bold', marginBottom: 8, color: 'black'  }}>Važi do</Text>
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
      fontSize: 16,
      color: 'black'

    },
    textLabels: {
      fontSize: 16,
      fontWeight: 'bold',
      marginBottom: 8
    }
  });

export default Profile;