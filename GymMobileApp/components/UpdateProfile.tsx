import React, { useState, useEffect } from 'react';
import { View, Text, TextInput, TouchableOpacity, Alert, StyleSheet } from 'react-native';
import Layout from './Layout';
import { NavigationProp, ParamListBase } from '@react-navigation/native';
import EncryptedStorage from 'react-native-encrypted-storage';
import axios from "axios";
import { KeyboardAwareScrollView } from 'react-native-keyboard-aware-scroll-view';
import { BASE_URL } from '../config/api-url.config';

type UpdateProfileProps = {
    navigation: NavigationProp<ParamListBase>;
};

const UpdateProfile = ({ navigation }: UpdateProfileProps) => {
  const profileUrl = `${BASE_URL}/App`;
  const [userData, setUserData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    address: ''
  });

  useEffect(() => {
    const fetchData = async () => {
      const gymUserId = await EncryptedStorage.getItem('gym_user_id');
      const response = await axios.get(`${profileUrl}/${gymUserId}`);

      if (response.status === 200) {
        setUserData(response.data);
      } else {
        Alert.alert(response.data.message);
      }
    };

    fetchData();
  }, []);

  const handleUpdateUserData = () => {
    const fetchData = async () => {
      try {
        const gymUserId = await EncryptedStorage.getItem('gym_user_id');
        const response = await axios.put(`${profileUrl}/${gymUserId}`, {
          email: userData.email,
          firstName: userData.firstName,
          lastName: userData.lastName,
          address: userData.address
        });
  
        if (response.status === 200) {
            navigation.navigate('Profile');
            Alert.alert("Profil je uspiješno ažuriran");
        } else {
          Alert.alert(response.data.message);
        }
      } catch (error: any) {
        if (error.response && (error.response.status === 400))
          Alert.alert(error.response.data.message);
        else
          Alert.alert("Došlo je do greške prilikom ažuriranja podataka");
      }
    };
  
      if (userData.email.trim() === "") {
        Alert.alert("Email je obavezno polje");
        return;
      }
      const emailRegex = /\S+@\S+\.\S+/;
      if (!emailRegex.test(userData.email)) {
        // Invalid email format
        Alert.alert("Email nije validan");
        return;
      }

      if (userData.firstName.trim()  === "") {
        // One of the fields is empty
        Alert.alert("Ime ne moze biti prazno");
        return;
      }

      if (userData.lastName.trim() === "") {
        // One of the fields is empty
        Alert.alert("Prezime ne moze biti prazno");
        return;
      }

    fetchData();
  };

  return (
    <Layout navigation={navigation}>
      <KeyboardAwareScrollView enableOnAndroid={true}>
        <View style={styles.container}>
          <Text style={styles.label}>Ime</Text>
          <TextInput
            placeholder="Unesite ime"
            placeholderTextColor={"black"}
            value={userData.firstName}
            onChangeText={(text) => setUserData({ ...userData, firstName: text })}
            style={styles.input}
          />

          <Text style={styles.label}>Prezime</Text>
          <TextInput
            placeholder="Unesite prezime"
            placeholderTextColor={"black"}
            value={userData.lastName}
            onChangeText={(text) => setUserData({ ...userData, lastName: text })}
            style={styles.input}
          />

          <Text style={styles.label}>Email</Text>
          <TextInput
            placeholder="Unesite email"
            placeholderTextColor={"black"}
            value={userData.email}
            onChangeText={(text) => setUserData({ ...userData, email: text })}
            style={styles.input}
          />

          <Text style={styles.label}>Adresa</Text>
          <TextInput
            placeholder="Unesite adresu"
            placeholderTextColor={"black"}
            value={userData.address}
            onChangeText={(text) => setUserData({ ...userData, address: text })}
            style={styles.input}
          />

          <TouchableOpacity onPress={handleUpdateUserData}>
            <View style={styles.button}>
              <Text style={styles.buttonText}>Promijeni</Text>
            </View>
          </TouchableOpacity>
        </View>
      </KeyboardAwareScrollView>
    </Layout>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    paddingTop: 100,
    paddingLeft: 30,
    paddingRight: 30
  },
  label: {
    fontSize: 16,
    fontWeight: 'bold',
    marginBottom: 8,
    color: 'black'
  },
  input: {
    width: '100%',
    height: 40,
    borderWidth: 1,
    borderColor: 'gray',
    borderRadius: 4,
    paddingHorizontal: 8,
    marginBottom: 16,
    fontSize: 16,
    color: 'black'
  },
  helperText: {
    fontSize: 12,
    color: 'gray',
    marginBottom: 8,
  },
  button: {
    width: '100%',
    height: 50,
    backgroundColor: '#2c3e50',
    borderRadius: 5,
    alignItems: 'center',
    justifyContent: 'center',
  },
  buttonText: {
    color: '#fff',
    fontSize: 16,
  }
});

export default UpdateProfile;
