import React, { useState, useEffect } from 'react';
import { View, Text, TextInput, TouchableOpacity, Alert } from 'react-native';
import Layout from './Layout';
import { NavigationProp, ParamListBase } from '@react-navigation/native';
import EncryptedStorage from 'react-native-encrypted-storage';
import axios from "axios";

type UpdateProfileProps = {
    navigation: NavigationProp<ParamListBase>;
};

const UpdateProfile = ({ navigation }: UpdateProfileProps) => {
  const [userData, setUserData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    address: ''
  });

  useEffect(() => {
    const fetchData = async () => {
      const gymUserId = await EncryptedStorage.getItem('gym_user_id');
      // android
      // const response = await axios.get(`http://10.0.2.2:42068/api/App/${gymUserId}`);
      const response = await axios.get(`http://localhost:42068/api/App/${gymUserId}`);

      if (response.status === 200) {
        setUserData(response.data);
      } else {
        // handle error
      }
    };

    fetchData();
  }, []);

  const handleUpdateUserData = () => {
    navigation.navigate('Profile');
    // const fetchData = async () => {
    //   try {
    //     const gymUserId = await EncryptedStorage.getItem('gym_user_id');
    //     // const response = await axios.put(`http://10.0.2.2:42068/api/App/${gymUserId}`, {
    //     const response = await axios.put(`http://localhost:42068/api/App/${gymUserId}`, {
    //       email: userData.email,
    //       firstName: userData.firstName,
    //       lastName: userData.lastName,
    //       address: userData.address
    //     });
  
    //     if (response.status === 200) {
    //         navigation.navigate('Profile');
    //         Alert.alert("Profil je uspijesno azuriran");
    //     } else {
    //       // handle error
    //     }
    //   } catch (error: any) {
    //     if (error.response && (error.response.status === 400))
    //       Alert.alert(error.response.data);
    //     else
    //       Alert.alert("Došlo je do greške prilikom prijavljivanja");
    //   }
    // };
  
    //   if (userData.email.trim() === "") {
    //     Alert.alert("Email ne moze biti prazni");
    //     return;
    //   }
    //   const emailRegex = /\S+@\S+\.\S+/;
    //   if (!emailRegex.test(userData.email)) {
    //     // Invalid email format
    //     Alert.alert("Email nije validan");
    //     return;
    //   }

    //   if (userData.firstName.trim()  === "") {
    //     // One of the fields is empty
    //     Alert.alert("Ime ne moze biti prazno");
    //     return;
    //   }

    //   if (userData.lastName.trim() === "") {
    //     // One of the fields is empty
    //     Alert.alert("Prezime ne moze biti prazno");
    //     return;
    //   }

    // fetchData();
  };

  return (
    <Layout navigation={navigation}>
    <View style={{ flex:1, padding: 16 }}>
      <Text style={{ fontSize: 16, fontWeight: 'bold', marginBottom: 8 }}>Ime</Text>
      <TextInput
        placeholder="Unesite ime"
        value={userData.firstName}
        onChangeText={(text) => setUserData({ ...userData, firstName: text })}
        style={{ fontSize: 16, marginBottom: 16 }}
      />

      <Text style={{ fontSize: 16, fontWeight: 'bold', marginBottom: 8 }}>Prezime</Text>
      <TextInput
        placeholder="Unesite prezime"
        value={userData.lastName}
        onChangeText={(text) => setUserData({ ...userData, lastName: text })}
        style={{ fontSize: 16, marginBottom: 16 }}
      />

      <Text style={{ fontSize: 16, fontWeight: 'bold', marginBottom: 8 }}>Email</Text>
      <TextInput
        placeholder="Unesite email"
        value={userData.email}
        onChangeText={(text) => setUserData({ ...userData, email: text })}
        style={{ fontSize: 16, marginBottom: 16 }}
      />

      <Text style={{ fontSize: 16, fontWeight: 'bold', marginBottom: 8 }}>Adresa</Text>
      <TextInput
        placeholder="Unesite adresu"
        value={userData.address}
        onChangeText={(text) => setUserData({ ...userData, address: text })}
        style={{ fontSize: 16, marginBottom: 32 }}
      />

      <TouchableOpacity onPress={handleUpdateUserData}>
        <View style={{ backgroundColor: '#2c3e50', borderRadius: 5, alignItems: 'center',  padding: 16 }}>
          <Text style={{ color: 'white', fontSize: 16, textAlign: 'center' }}>Promijeni</Text>
        </View>
      </TouchableOpacity>
    </View>
    </Layout>
  );
};

export default UpdateProfile;
