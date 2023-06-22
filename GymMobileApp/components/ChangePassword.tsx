import React, { useState } from 'react';
import { View, Text, TextInput, TouchableOpacity,Button, StyleSheet, Alert } from 'react-native';
import { NavigationProp, ParamListBase } from '@react-navigation/native';
import Layout from './Layout';
import EncryptedStorage from 'react-native-encrypted-storage';
import axios from "axios";
import { KeyboardAwareScrollView } from 'react-native-keyboard-aware-scroll-view';
import { BASE_URL } from '../config/api-url.config';

type ChangePasswordProps = {
    navigation: NavigationProp<ParamListBase>;
};

const ChangePassword = ({ navigation }: ChangePasswordProps) => {
  const changePassUrl = `${BASE_URL}/Auth/change-password`;
  const [currentPassword, setCurrentPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmNewPassword, setConfirmNewPassword] = useState('');

  const handleCurrentPasswordChange = (text: string) => {
    setCurrentPassword(text);
  };

  const handleNewPasswordChange = (text: string) => {
    setNewPassword(text);
  };

  const handleConfirmNewPasswordChange = (text: string) => {
    setConfirmNewPassword(text);
  };

  const handleSubmit = async () => {
    if (currentPassword.trim()  === "") {
      // One of the fields is empty
      Alert.alert("Trenutna lozinka se mora unijeti");
      return;
    }
    if (newPassword.trim()  === "") {
      // One of the fields is empty
      Alert.alert("Nova lozinka se mora unijeti");
      return;
    }
    const passwordRegex = /^(?=.*[A-Z])(?=.*\d).{8,}$/;
    if (!passwordRegex.test(newPassword)) {
      Alert.alert('Nova lozinka mora sadržati barem 8 znakova, jedno veliko slovo i jedan broj');
      return;
    }
    if (confirmNewPassword.trim()  === "") {
      // One of the fields is empty
      Alert.alert("Potvrda nove lozinke se mora unijeti");
      return;
    }
    if (newPassword !== confirmNewPassword){
      Alert.alert("Ne podudaraju se lozinke");
      return;
    }
    
    try {
      const userId = await EncryptedStorage.getItem('user_id');
      const response = await axios.post(changePassUrl, JSON.stringify({
        id: userId,
        currentPassword: currentPassword,
        newPassword: newPassword,
        confirmNewPassword: confirmNewPassword
      }), { headers: {"content-type": "application/json" }});

      if (response.status === 200) {
        navigation.navigate('Profile');
        Alert.alert("Lozinka je uspijesno promijenjena");
      } else {
        Alert.alert(response.data.message);
      }
    } catch (error: any) {
      if (error.response && (error.response.status === 400))
        Alert.alert(error.response.data.message);
      else
        Alert.alert("Došlo je do greške prilikom promjene lozinke");
    }
  };

  return (
    <Layout navigation={navigation}>
      <KeyboardAwareScrollView>
      <View style={styles.container}>
        <Text style={styles.label}>Trenutna lozinka</Text>
        <TextInput
          style={styles.input}
          secureTextEntry={true}
          value={currentPassword}
          onChangeText={handleCurrentPasswordChange}
        />
        <Text style={styles.label}>Nova lozinka</Text>
        <Text style={styles.helperText}>Mora sadržati barem 8 karaktera, jedno veliko slovo i jedan broj</Text>
        <TextInput
          style={styles.input}
          secureTextEntry={true}
          value={newPassword}
          onChangeText={handleNewPasswordChange}
        />
        <Text style={styles.label}>Potvrdite novu lozinku</Text>
        <TextInput
          style={styles.input}
          secureTextEntry={true}
          value={confirmNewPassword}
          onChangeText={handleConfirmNewPasswordChange}
        />
      <TouchableOpacity style={styles.button} onPress={handleSubmit}>
        <Text style={styles.buttonText}>Promijeni</Text>
      </TouchableOpacity>
      </View>
      </KeyboardAwareScrollView>
    </Layout>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
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
    color: 'black'
  },
  helperText: {
    fontSize: 12,
    color: 'gray',
    marginBottom: 8,
  },
  button: {
    width: '80%',
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

export default ChangePassword;
