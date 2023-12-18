﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using efcoreApp.Data;

#nullable disable

namespace efcoreApp.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20231218161528_AddTableTeacher")]
    partial class AddTableTeacher
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.8");

            modelBuilder.Entity("efcoreApp.Data.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("TeacherId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("CourseId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("efcoreApp.Data.CourseRegistration", b =>
                {
                    b.Property<int>("RegistrationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("StudentId")
                        .HasColumnType("INTEGER");

                    b.HasKey("RegistrationId");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId");

                    b.ToTable("CourseRegistrations");
                });

            modelBuilder.Entity("efcoreApp.Data.Student", b =>
                {
                    b.Property<int>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT");

                    b.Property<string>("StudentAd")
                        .HasColumnType("TEXT");

                    b.Property<string>("StudentSoyad")
                        .HasColumnType("TEXT");

                    b.HasKey("StudentId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("efcoreApp.Data.Teacher", b =>
                {
                    b.Property<int>("TeacherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartingDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("TeacherAd")
                        .HasColumnType("TEXT");

                    b.Property<string>("TeacherSoyad")
                        .HasColumnType("TEXT");

                    b.HasKey("TeacherId");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("efcoreApp.Data.Course", b =>
                {
                    b.HasOne("efcoreApp.Data.Teacher", "Teacher")
                        .WithMany("Courses")
                        .HasForeignKey("TeacherId");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("efcoreApp.Data.CourseRegistration", b =>
                {
                    b.HasOne("efcoreApp.Data.Course", "Course")
                        .WithMany("CourseRegistrations")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("efcoreApp.Data.Student", "Student")
                        .WithMany("CourseRegistrations")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("efcoreApp.Data.Course", b =>
                {
                    b.Navigation("CourseRegistrations");
                });

            modelBuilder.Entity("efcoreApp.Data.Student", b =>
                {
                    b.Navigation("CourseRegistrations");
                });

            modelBuilder.Entity("efcoreApp.Data.Teacher", b =>
                {
                    b.Navigation("Courses");
                });
#pragma warning restore 612, 618
        }
    }
}
