import React, { useState } from 'react';
import { View, Text, TextInput, TouchableOpacity,Button, StyleSheet, Alert } from 'react-native';
import { NavigationProp, ParamListBase } from '@react-navigation/native';
import Layout from './Layout';
import EncryptedStorage from 'react-native-encrypted-storage';
import axios from "axios";
import { panHandlerName } from 'react-native-gesture-handler/lib/typescript/handlers/PanGestureHandler';

type ChangePasswordProps = {
    navigation: NavigationProp<ParamListBase>;
};

const ChangePassword = ({ navigation }: ChangePasswordProps) => {
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
    navigation.navigate('Profile');
    // Handle password change
    // const fetchData = async () => {
    //   try {
    //     const userId = await EncryptedStorage.getItem('user_id');
    //     // android:
    //     // const response = await axios.post(`http://10.0.2.2:42068/api/Auth/change-password`, {
    //     const response = await axios.post(`http://localhost:42068/api/Auth/change-password`, {
    //       id: userId,
    //       currentPassword: currentPassword,
    //       newPassword: newPassword,
    //       confirmNewPassword: confirmNewPassword
    //     });

    //     if (response.status === 200) {
    //       navigation.navigate('Profile');
    //       Alert.alert("Lozinka je uspijesno promijenjena");
    //     } else {
    //       Alert.alert(response.data);
    //     }
    //   } catch (error: any) {
    //     if (error.response && error.response.status === 400) {
    //       Alert.alert(error.response.data);
    //     } else {
    //       Alert.alert("Došlo je do pogreške prilikom promjene lozinke.");
    //     }
    //   }
    // };

    // if (currentPassword.trim()  === "") {
    //   // One of the fields is empty
    //   Alert.alert("Trenutna lozinka se mora unijeti");
    //   return;
    // }
    // if (newPassword.trim()  === "") {
    //   // One of the fields is empty
    //   Alert.alert("Nova lozinka se mora unijeti");
    //   return;
    // }
    // const passwordRegex = /^(?=.*[A-Z])(?=.*\d).{8,}$/;
    // if (!passwordRegex.test(newPassword)) {
    //   Alert.alert('Nova lozinka mora sadržati barem 8 znakova, jedno veliko slovo i jedan broj');
    //   return;
    // }
    // if (confirmNewPassword.trim()  === "") {
    //   // One of the fields is empty
    //   Alert.alert("Potvrda nove lozinke se mora unijeti");
    //   return;
    // }
    // if (newPassword !== confirmNewPassword){
    //   Alert.alert("Ne podudaraju se lozinke");
    //   return;
    // }
    // await fetchData();
  };

  return (
    <Layout navigation={navigation}>
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
        <Text style={styles.label}>Potvrdi trenutnu lozinku</Text>
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
    </Layout>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    padding: 16,
    backgroundColor: '#fff',
  },
  label: {
    fontSize: 16,
    fontWeight: 'bold',
    marginBottom: 8,
  },
  input: {
    width: '100%',
    height: 40,
    borderWidth: 1,
    borderColor: 'gray',
    borderRadius: 4,
    paddingHorizontal: 8,
    marginBottom: 16,
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
